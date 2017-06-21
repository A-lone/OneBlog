using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriters;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using One.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using One.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Qiniu.Conf;

namespace Json.Test
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static QiniuService service;
        static ApplicationContext context;
        //http://stackoverflow.com/questions/37526165/compiling-and-running-code-at-runtime-in-net-core-1-0
        //http://www.cnblogs.com/zkweb/p/5857355.html
        //http://www.cnblogs.com/foreachlife/p/ciiproslyn.html
        //http://www.cnblogs.com/TianFang/p/3649799.html
        static void Main(string[] args)
        {

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Console.WriteLine("Hello World!");

            var fileName = "2.html";

            Config.ACCESS_KEY = Configuration["Qiniu:AccessKey"];
            Config.SECRET_KEY = Configuration["Qiniu:SecretKey"];
            Config.UP_HOST = Configuration["Qiniu:UP_Host"];

            var path = Path.GetFullPath(fileName);
            string result = string.Empty;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                byte[] heByte = new byte[stream.Length];
                int r = stream.Read(heByte, 0, heByte.Length);
                result = System.Text.Encoding.UTF8.GetString(heByte);
            }
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(result);


            service = new QiniuService(Configuration);
            DbContextOptions<ApplicationContext> dbContextOption = new DbContextOptions<ApplicationContext>();
            DbContextOptionsBuilder<ApplicationContext> dbContextOptionBuilder = new DbContextOptionsBuilder<ApplicationContext>(dbContextOption);
            context = new ApplicationContext(dbContextOption, Configuration);
            DG(doc.DocumentNode.ChildNodes).Wait();
            //Regex reg = new Regex("<li class=\"col-sm-6 col-md-4 col-lg-3 mbm\">[^'\"\"\\s>]+</li>");
            //MatchCollection mc = reg.Matches(result);
            //foreach (Match m in mc)
            //{
            //    Console.WriteLine(m.Groups["url"].Value + "\n");
            //    Console.WriteLine(m.Groups["text"].Value + "\n");
            //}
            //var code = JsonText();
            //CompileAndRun(code);
        }


        public static async Task<string> UrlDownload(string url)
        {
            HttpClient client = new HttpClient();
            var resp = await client.GetAsync(url);

            var buffer = await resp.Content.ReadAsByteArrayAsync();

            url = await service.Upload(buffer);

            return url;
        }

        static async Task DG(HtmlNodeCollection collection)
        {

            foreach (var item in collection)
            {
                await DG(item.ChildNodes);

                if (item.Name == "ul")
                {
                    var clazz = item.Attributes["class"];
                    if (clazz != null && clazz.Value.Contains("indexlist"))
                    {
                        var apps = new List<StoreApp>();
                        var cate = context.StoreCategories.Where(m => m.Title == "实用程序与工具").FirstOrDefault();
                        foreach (var i1 in item.ChildNodes)
                        {
                            if (i1.Name == "li")
                            {
                                var app = new StoreApp();
                                app.PDB = i1.ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["href"].Value;
                                app.ProductId = app.PDB.Replace("ms-windows-store://pdp/?ProductId=", "");
                                app.Icon = i1.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[0].Attributes["src"].Value;
                                app.Icon = await UrlDownload(app.Icon);
                                app.Categories = cate;
                                app.Description = i1.ChildNodes[1].ChildNodes[1].ChildNodes[7].ChildNodes[0].InnerHtml;
                                app.AppName = i1.ChildNodes[1].ChildNodes[1].ChildNodes[3].ChildNodes[0].InnerHtml;
                                context.StoreApp.Add(app);
                            }

                        }
                        context.SaveChanges();
                        break;
                    }
                }
            }
        }

        static void CC(string code)
        {

            var compilation = CSharpCompilation.Create("test").WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(JsonProperty).GetTypeInfo().Assembly.Location));

            code = @"
using System;

public static class C
{
    public static void M()
    {
        Console.WriteLine(""Hello Roslyn."");
    }
}";

            var parseOptions = CSharpParseOptions.Default;
            parseOptions = parseOptions.WithPreprocessorSymbols("NETCORE");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            var reference = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (var item in reference)
            {
                compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(item).Location));
            }


            compilation.AddSyntaxTrees(syntaxTree);

            var fileName = "test.dll";

            var result = compilation.Emit(fileName);

            var a = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(fileName));

            var type = a.GetType("RootObject");

            var obj = Activator.CreateInstance(type);
        }

        static void CompileAndRun(string code)
        {
            //    var text = @"
            //public class Calculator
            //{
            //    public static int Evaluate() { return 3 + 2 * 5; }
            //}";


            var text = code;
            var tree = SyntaxFactory.ParseSyntaxTree(text);

            List<MetadataReference> list = new List<MetadataReference>();
            list.Add(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location));
            list.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")).Location));

            var reference = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            foreach (var item in reference)
            {
                list.Add(MetadataReference.CreateFromFile(Assembly.Load(item).Location));
                //compilation.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(item).Location));
            }

            var compilation = CSharpCompilation.Create("data1.dll", new[] { tree },
             options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
             references: list);//new[] { MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location) }

            //Assembly compiledAssembly;
            //using (var stream = new MemoryStream())
            //{
            //    var compileResult = compilation.Emit(stream);
            //    compiledAssembly = Assembly.Load(stream.GetBuffer());
            //}

            var fileName = "data1.dll";

            var compileResult = compilation.Emit(fileName);
            var compiledAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(fileName));

            var RootObject = compiledAssembly.GetType("MockData.RootObject");

            var obj = Activator.CreateInstance(RootObject);
        }



        static string JsonText()
        {
            var gen = new JsonClassGenerator();
            gen.Example = "{\"errno\":0,\"data\":{\"sum\":18,\"list\":[{\"qid\":186,\"createTime\":\"2017-03-24 16:27:23\",\"title\":\"\u53c8\u4e00\u660e\u661f\u5bb6\u4ea7\u906d\u5a07\u59bb\u638f\u7a7a\uff0c\u8001\u5c11\u914d\u4e3a\u4f55\u4e0d\u9760\u8c31\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":185,\"createTime\":\"2017-03-24 16:26:17\",\"title\":\"\u5e1d\u90fd\u56db\u73af\u8fb9\u8d2d\u623f\u7684\u9996\u4ed8\u771f\u76f8\uff0c\u4e0d\u5403\u4e0d\u559d\u9700\u8981\u6512\u591a\u5c11\u5e74\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":184,\"createTime\":\"2017-03-24 16:25:10\",\"title\":\"\",\"status\":2,\"uid\":125498771,\"type\":\"\u6765\u81ea \u672a\u77e5 \u7f16\u8f91\u5668\"},{\"qid\":183,\"createTime\":\"2017-03-24 16:24:21\",\"title\":\"\u5973\u4eba\u5a5a\u540e\u4f1a\u6539\u53d8\uff0c\u90fd\u662f\u7537\u4eba\u60f9\u7684\u7978\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":182,\"createTime\":\"2017-03-24 16:22:33\",\"title\":\"\u65b0\u6b22\uff1f\u65e7\u7231\uff1f\u76f4\u5230\u6d41\u6cea\u7684\u90a3\u4e00\u523b\uff0c\u6211\u624d\u53d1\u73b0\u8fd8\u6709\u7b2c\u4e09\u79cd\u9009\u62e9\u2026\u2026\",\"status\":22,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":16,\"createTime\":\"2017-03-24 14:19:37\",\"title\":\"\u5982\u4f55\u770b\u5230\u8fbe\u5185\u548c\u706b\u661f\u65f6\u4ee3\u4e24\u5bb6\u673a\u6784\",\"status\":21,\"uid\":1297457125,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":14,\"createTime\":\"2017-03-23 18:44:15\",\"title\":\"\u51cf\u80a5\u7ecf\u9a8c\u5206\u4eab \u975e\u5e38\u5b9e\u7528\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":12,\"createTime\":\"2017-03-22 16:09:50\",\"title\":\"\u8fd9\u4e2a\u662f\u6d4b\u8bd5\u6570\u636e-1\",\"status\":21,\"uid\":1297457125,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":11,\"createTime\":\"2017-03-21 19:27:53\",\"title\":\"\u8fd9\u662f\u6d4b\u8bd5test\",\"status\":11,\"uid\":1297457125,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":10,\"createTime\":\"2017-03-21 18:02:22\",\"title\":\"\u201c9-11\u201d\u6050\u6016\u88ad\u51fb\u9047\u96be\u8005\u5bb6\u5c5e\u96c6\u4f53\u8d77\u8bc9\u6c99\u7279\u653f\u5e9c\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"}]},\"errmsg\":\"\u64cd\u4f5c\u6210\u529f\"}";//json
            gen.InternalVisibility = false;
            gen.CodeWriter = new CSharpCodeWriter();
            gen.ExplicitDeserialization = false;
            gen.Namespace = "MockData";
            gen.NoHelperClass = false;
            gen.SecondaryNamespace = null;
            gen.UseProperties = true;
            gen.MainClass = "RootObject";
            gen.UsePascalCase = true;
            gen.UseNestedClasses = true;
            gen.ApplyObfuscationAttributes = false;
            gen.ExamplesInDocumentation = false;
            gen.TargetFolder = null;
            gen.SingleFile = true;
            string lastGeneratedString = null;
            using (var sw = new StringWriter())
            {
                gen.OutputStream = sw;
                gen.GenerateClasses();
                sw.Flush();
                lastGeneratedString = sw.ToString();
            }
            return lastGeneratedString;
        }
    }
}