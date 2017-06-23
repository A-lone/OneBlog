using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.WebEncoders;
using OneBlog.Data;
using OneBlog.Data.Common;
using OneBlog.Data.Contracts;
using OneBlog.Data.Repository;
using OneBlog.Helpers;
using OneBlog.Logger;
using OneBlog.MetaWeblog;
using OneBlog.Mvc;
using OneBlog.Services;
using OneBlog.Services.DataProviders;
using OneBlog.Settings;
using Qiniu.Conf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace OneBlog
{
    public class Startup
    {
        private IConfigurationRoot _config;
        private IHostingEnvironment _env;
        public static readonly IEnumerable<string> MimeTypes = new[]{
    // General
    "text/plain",
    // Static files
    "text/css",
    "application/javascript",
    // MVC
    "text/html",
    "application/xml",
    "text/xml",
    "application/json",
    "text/json",};

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", false, true)
              .AddEnvironmentVariables();

            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection svcs)
        {
            svcs.AddTimedJob();
            svcs.AddMvcDI();
            AspNetCoreHelper.ConfigureServices(svcs);

            svcs.Configure<AppSettings>(_config.GetSection("AppSettings"));
            
            svcs.AddSession();
            svcs.AddResponseCompression();
            svcs.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            svcs.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                         {
                               "image/svg+xml",
                               "application/atom+xml"
                            }); ;
                options.Providers.Add<GzipCompressionProvider>();
            });

            svcs.AddSingleton(_config);

            if (_env.IsDevelopment())
            {
                svcs.AddTransient<IMailService, LoggingMailService>();
            }
            else
            {
                svcs.AddTransient<IMailService, MailService>();
            }

            svcs.AddDbContext<ApplicationContext>(ServiceLifetime.Scoped);

            svcs.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationContext>();

            svcs.AddScoped<IPostsRepository, PostsRepository>();
            svcs.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            svcs.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
            });

            svcs.AddScoped<IStoreRepository, StoreRepository>();
            svcs.AddScoped<IViewRenderService, ViewRenderService>();
            svcs.AddScoped<ICommentsRepository, CommentsRepository>();
            svcs.AddScoped<ITagsRepository, TagsRepository>();
            svcs.AddScoped<IRolesRepository, RolesRepository>();
            svcs.AddScoped<ILookupsRepository, LookupsRepository>();
            svcs.AddScoped<ICategoriesRepository, CategoriesRepository>();
            svcs.AddScoped<IUsersRepository, UsersRepository>();

            svcs.AddTransient<JsonService>();
            svcs.AddTransient<ApplicationInitializer>();
            svcs.AddScoped<AdService>();
            svcs.AddScoped<QiniuService>();
            svcs.AddScoped<NavigationHelper>();
            // Data Providers (non-EF)
            svcs.AddScoped<CalendarProvider>();
            svcs.AddScoped<CoursesProvider>();
            svcs.AddScoped<PublicationsProvider>();
            svcs.AddScoped<PodcastEpisodesProvider>();
            svcs.AddScoped<VideosProvider>();
            svcs.AddTransient<ApplicationEnvironment>();

            // Supporting Live Writer (MetaWeblogAPI)
            svcs.AddMetaWeblog<WeblogProvider>();

            //svcs.Configure<MvcOptions>(options => {
            //    options.InputFormatters.OfType<JsonInputFormatter>().First().SupportedMediaTypes.Add(
            //        new MediaTypeHeaderValue("application/vnd.myget.webhooks.v1+json")
            //    );
            //});

            // Add Caching Support
            svcs.AddMemoryCache(opt => opt.ExpirationScanFrequency = TimeSpan.FromMinutes(5));

            //// Add MVC to the container
            var mvcBuilder = svcs.AddMvc();
            //mvcBuilder.AddJsonOptions(opts => opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
            mvcBuilder.AddJsonOptions(r =>
            {
                r.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                r.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            //var mvcCore = svcs.AddMvcCore();
            //mvcCore.AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver());
            // Add Https - renable once Azure Certs work
            //if (_env.IsProduction()) mvcBuilder.AddMvcOptions(options => options.Filters.Add(new RequireHttpsAttribute()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory,
                              IMailService mailService,
                              IServiceScopeFactory scopeFactory)
        {

            app.UseTimedJob();
            app.UseMvcDI();
            app.UseResponseCompression();
            AspNetCoreHelper.Configure(app, env, loggerFactory);
            app.UseSession();
            // Add the following to the request pipeline only in development environment.
            if (_env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information);
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // Support logging to email
                loggerFactory.AddEmail(mailService, LogLevel.Critical);
                loggerFactory.AddConsole(LogLevel.Error);

                // Early so we can catch the StatusCode error
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Exception");
            }

            // Rewrite old URLs to new URLs
            //app.UseUrlRewriter();

            app.UseStaticFiles();

            // Support MetaWeblog API
            app.UseMetaWeblog("/livewriter");


            Config.ACCESS_KEY = _config["Qiniu:AccessKey"];
            Config.SECRET_KEY = _config["Qiniu:SecretKey"];
            Config.UP_HOST = _config["Qiniu:UP_Host"];

            // Keep track of Active # of users for Vanity Project
            app.UseMiddleware<ActiveUsersMiddleware>();

            app.UseIdentity();

            app.UseMvc();
            if (_config["OneDb:TestData"] != "True")
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var initializer = scope.ServiceProvider.GetService<ApplicationInitializer>();
                    initializer.SeedAsync().Wait();
                }
            }
        }
    }
}
