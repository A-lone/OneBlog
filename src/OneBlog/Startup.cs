using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.WebEncoders;
using OneBlog.Configuration;
using OneBlog.Data;
using OneBlog.Data.Common;
using OneBlog.Data.Contracts;
using OneBlog.Data.Repository;
using OneBlog.Helpers;
using OneBlog.Logger;
using OneBlog.MetaWeblog;
using OneBlog.Mvc;
using OneBlog.Services;
using Qiniu.Conf;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace OneBlog
{
    public class Startup
    {

        private IHostingEnvironment _env { get; }
        private IConfiguration _conf { get; }


        public Startup(IHostingEnvironment env, IConfiguration conf)
        {
            //中文支持
            //EncodingProvider provider = CodePagesEncodingProvider.Instance;
            //Encoding.RegisterProvider(provider);
            _env = env;
            _conf = conf;
        }


        public void ConfigureServices(IServiceCollection svcs)
        {
            svcs.AddTimedJob();
            svcs.AddMvcDI();
            AspNetCoreHelper.ConfigureServices(svcs);
            svcs.Configure<AppSettings>(_conf.GetSection(nameof(AppSettings)));
            svcs.Configure<DataSettings>(_conf.GetSection(nameof(DataSettings)));
            svcs.Configure<QiniuSettings>(_conf.GetSection(nameof(QiniuSettings)));
            svcs.Configure<EditorSettings>(_conf.GetSection(nameof(EditorSettings)));

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


            if (_env.IsDevelopment())
            {
                svcs.AddTransient<IMailService, LoggingMailService>();
            }
            else
            {
                svcs.AddTransient<IMailService, MailService>();
            }

            svcs.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);

            svcs.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();


            svcs.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            svcs.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
            });

            svcs.AddScoped<IPostsRepository, PostsRepository>();
            svcs.AddScoped<IDashboardRepository, DashboardRepository>();
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
            svcs.AddScoped<QiniuService>();
            svcs.AddScoped<NavigationHelper>();
            svcs.AddTransient<ApplicationEnvironment>();

            svcs.AddTransient<IDbContextFactory, DbContextFactory>();
            // Supporting Live Writer (MetaWeblogAPI)
            svcs.AddMetaWeblog<WeblogProvider>();

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
            if (_env.IsProduction())
            {
                mvcBuilder.AddMvcOptions(options => options.Filters.Add(new RequireHttpsAttribute()));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              ILoggerFactory loggerFactory,
                              IMailService mailService,
                              IServiceScopeFactory scopeFactory)
        {

            app.UseTimedJob();
            app.UseMvcDI();
            app.UseResponseCompression();
            AspNetCoreHelper.Configure(app, _env, loggerFactory);
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

            // Keep track of Active # of users for Vanity Project
            app.UseMiddleware<ActiveUsersMiddleware>();

            app.UseAuthentication();

            app.UseMvc();

            if (_conf["OneDb:TestData"] != "True")
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
