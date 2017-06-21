using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using One.Mock.Data;
using Microsoft.EntityFrameworkCore;
using One.Mock.Repositories;
using Newtonsoft.Json.Serialization;

namespace One.Mock
{
    //https://damienbod.com/2015/08/30/asp-net-5-with-sqlite-and-entity-framework-7/
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["Production:SqliteConnectionString"];
            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlite(connection)
           );
            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.AddScoped<IDataEventRecordRepository, DataEventRecordRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<ISitePathRepository, SitePathRepository>();
            services.AddTransient<ApplicationInitializer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            app.UseMiddleware<CustomErrorPagesMiddleware>();
            app.UseMiddleware<MockMiddleware>();
            app.UseMvc();
            using (var scope = scopeFactory.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetService<ApplicationInitializer>();
                initializer.SeedAsync().Wait();
            }
        }
    }
}
