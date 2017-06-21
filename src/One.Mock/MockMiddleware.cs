using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using One.Mock.Controllers;
using One.Mock.Data;
using One.Mock.Model;
using One.Mock.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace One.Mock
{
    public class MockMiddleware
    {
        private ILogger _logger;
        private readonly RequestDelegate _next;
        private Site _site;
        private IHostingEnvironment _env;
        private ISitePathRepository _sitePathRepository;
        private ISiteRepository _siteRepository;
        private ApplicationContext _ctx;

        public MockMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
            IHostingEnvironment env,
            ISitePathRepository sitePathRepository,
            ApplicationContext ctx,
            ISiteRepository siteRepository)
        {
            _ctx = ctx;
            _env = env;
            _next = next;
            _sitePathRepository = sitePathRepository;
            _siteRepository = siteRepository;
            _logger = loggerFactory.CreateLogger<MockMiddleware>(); ;
        }

        public void init()
        {
            if (_site == null)
            {
                _site = _siteRepository.GetDefault();
            }
        }

        public Type Load(string dll)
        {
            string fileName = Path.Combine(_env.ContentRootPath, "dll/" + dll);

            var compiledAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(fileName));

            var RootObject = compiledAssembly.GetType("MockData.RootObject");

            return RootObject;
            //var obj = Activator.CreateInstance(RootObject);
        }

        public async Task Invoke(HttpContext context)
        {
            //不为mock后台就执行
            if (!context.Request.Path.Value.Contains("mock/admin"))
            {
                init();
                _logger.LogInformation($"Request Mock");

                HttpResponseMessage message = null;

                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                Uri uri = new Uri(_site.Url);
                if (!string.IsNullOrEmpty(_site.Cookie))
                {
                    JsonMockService.AddCookieToContainer(_site.Cookie, cookies, uri.Host);
                }
                HttpClient client = new HttpClient(handler);

                string url = _site.Url + context.Request.Path + context.Request.QueryString;

                string contentType = string.Empty;


                if (context.Request.ContentType != null)
                {
                    contentType = context.Request.ContentType;
                }


                SitePath item = null;

                item = _site.SitePaths.FirstOrDefault(m => m.Path.Equals(context.Request.Path, StringComparison.OrdinalIgnoreCase)
                 && m.Method.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase));
                _logger.LogInformation($"Response Mock");
                byte[] buffer = null;
                int statusCode = 200;

                if (item != null && !item.RequestEnabled && !string.IsNullOrEmpty(item.Json))
                {
                    buffer = Encoding.UTF8.GetBytes(item.Json);
                    contentType = "application/json";
                }
                else
                {
                    if (context.Request.Method == "POST")
                    {
                        var httpContext = context.Request.HttpContext;

                        using (var multipartFormDataContent = new MultipartFormDataContent())
                        {
                            foreach (var keyValuePair in httpContext.Request.Form)
                            {
                                multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                                    String.Format("\"{0}\"", keyValuePair.Key));
                            }
                            //multipartFormDataContent.Add(new ByteArrayContent(File.ReadAllBytes("test.txt")),
                            //    '"' + "File" + '"',
                            //    '"' + "test.txt" + '"');

                            message = await client.PostAsync(url, multipartFormDataContent);
                        }
                    }
                    else if (context.Request.Method == "GET")
                    {
                        message = await client.GetAsync(url);
                    }
                    buffer = await message.Content.ReadAsByteArrayAsync();
                    statusCode = (int)message.StatusCode;
                    if (message.Content.Headers.ContentType != null)
                    {
                        contentType = message.Content.Headers.ContentType.MediaType;
                    }
                    else
                    {
                        contentType = message.Content.Headers.GetValues("Content-Type").FirstOrDefault();
                    }
                    if (contentType.Contains("application/json"))
                    {
                        if (item != null)
                        {
                            try
                            {
                                var json = Encoding.UTF8.GetString(buffer);

                                object obj = null;

                                var type = Load(item.DLL);

                                obj = JsonConvert.DeserializeObject(json, type);
                                if (!string.IsNullOrEmpty(item.Expression))
                                {
                                    try
                                    {
                                        var expression = JsonConvert.DeserializeObject<List<Expression>>(item.Expression);
                                        foreach (var e in expression)
                                        {
                                            if (!string.IsNullOrEmpty(e.IFKey))
                                            {
                                                var ifkey = JsonMockService.GetPropValue<string>(obj, e.IFKey);
                                                if (ifkey == e.IFValue)
                                                {
                                                    obj = JsonMockService.SetPropValue(obj, e.Key, e.Value);
                                                }
                                            }
                                            else
                                            {
                                                obj = JsonMockService.SetPropValue(obj, e.Key, e.Value);
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                                var text = JsonConvert.SerializeObject(obj);
                                buffer = Encoding.UTF8.GetBytes(text);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }

                    //PropertyInfo[] pis = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                }


                //var result = "<methodResponse><params><param><value><array><data><value><struct><member><name>blogid</name><value><string>stw</string></value></member><member><name>url</name><value><string>/</string></value></member><member><name>blogName</name><value><string>Shawn Wildermuth's Rants and Raves</string></value></member></struct></value></data></array></value></param></params></methodResponse>";
                context.Response.OnStarting((state) =>
                {
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        context.Response.ContentType = contentType;
                    }
                    context.Response.StatusCode = statusCode;
                    var responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                    foreach (Cookie cookie in responseCookies)
                    {
                        context.Response.Cookies.Append(cookie.Name, cookie.Value);
                    }

                    context.Response.Body.Write(buffer, 0, buffer.Length);
                    return Task.CompletedTask;
                }, null);


            }


            await _next.Invoke(context);
        }
    }
}
