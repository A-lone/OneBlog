using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace OneBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .UseStartup<Startup>()
                .Build();
    }
}
