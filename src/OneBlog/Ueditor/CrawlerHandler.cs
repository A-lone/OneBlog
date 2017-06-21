using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using OneBlog.Helpers;
using OneBlog.Services;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace OneBlog.UEditor
{
    /// <summary>
    /// Crawler 的摘要说明
    /// </summary>
    public class CrawlerHandler : IHandler
    {
        private StringValues Sources;
        private Crawler[] Crawlers;

        public object Process()
        {
            AspNetCoreHelper.HttpContext.Request.Form.TryGetValue("source[]", out Sources);

            if (Sources.Count == 0)
            {
                return (new
                {
                    state = "参数错误：没有指定抓取源"
                });

            }
            Crawlers = Sources.Select(x => new Crawler(x).Fetch()).ToArray();
            return (new
            {
                state = "SUCCESS",
                list = Crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            });
        }
    }

    public class Crawler
    {
        public string SourceUrl { get; set; }
        public string ServerUrl { get; set; }
        public string State { get; set; }


        public Crawler(string sourceUrl)
        {
            this.SourceUrl = sourceUrl;
        }

        public Crawler Fetch()
        {
            if (!IsExternalIPAddress(this.SourceUrl))
            {
                State = "INVALID_URL";
                return this;
            }
            HttpClient client = new HttpClient();
            var response = client.GetAsync(this.SourceUrl).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                State = "Url returns " + response.StatusCode + ", " + response.StatusCode;
                return this;
            }
            if (response.Content.Headers.ContentType.MediaType.IndexOf("image") == -1)
            {
                State = "Url is not an image";
                return this;
            }
            try
            {

                var qiniuService = DI.ServiceProvider.GetRequiredService<QiniuService>();
                var result = qiniuService.Upload(response.Content).Result;

                if (!string.IsNullOrEmpty(result))
                {
                    State = "SUCCESS";
                    ServerUrl = result;
                }
                else
                {
                    State = "上传错误";
                }
            }
            catch (Exception ex)
            {
                State = "抓取错误：" + ex.Message;
            }
            return this;

        }

        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntryAsync(uri.DnsSafeHost).Result;
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }

        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }
    }
}