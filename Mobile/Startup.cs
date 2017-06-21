using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using One.Data.Contracts;
using One.Data;
using Microsoft.DotNet.InternalAbstractions;
using One.Services;
using One.Data.Common;
using One.Data.Repository;
using Microsoft.Extensions.WebEncoders;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using One.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Mobile
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("config.json", false, true)
                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection svcs)
        {
            svcs.AddMvcDI();
            AspNetCoreHelper.ConfigureServices(svcs);
            svcs.AddSession();


            svcs.AddSingleton(Configuration);

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
            svcs.AddScoped<IViewRenderService, ViewRenderService>();
            svcs.AddScoped<IStoreRepository, StoreRepository>();
            svcs.AddScoped<ICommentsRepository, CommentsRepository>();
            svcs.AddScoped<ITagsRepository, TagsRepository>();
            svcs.AddScoped<IRolesRepository, RolesRepository>();
            svcs.AddScoped<ILookupsRepository, LookupsRepository>();
            svcs.AddScoped<ICategoriesRepository, CategoriesRepository>();
            svcs.AddScoped<IUsersRepository, UsersRepository>();

            svcs.AddTransient<JsonService>();
            svcs.AddTransient<ApplicationInitializer>();
            svcs.AddScoped<QiniuService>();
            // Data Providers (non-EF)


            // Supporting Live Writer (MetaWeblogAPI)

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseStaticFiles();

            app.UseMvcDI();

            app.UseMvc();
        }
    }
}
