using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneBlog.Configuration;

namespace OneBlog.Data
{
    // HACK to get Migrations to work.
    public class Startup
    {
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", false, true)
              .AddEnvironmentVariables();

            _config = builder.Build();

        }

        public void ConfigureServices(IServiceCollection svcs)
        {
            svcs.AddSingleton<IConfigurationRoot>(_config);
            svcs.Configure<DataConfiguration>(_config.GetSection("Data"));
            svcs.AddEntityFrameworkSqlite().AddDbContext<ApplicationContext>();
            //svc.AddEntityFrameworkSqlServer().AddDbContext<ApplicationContext>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentity();
        }
    }
}
