using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneBlog.Data;
using OneBlog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var dataProviderConfig = configuration.GetSection("Data")["Provider"];
            var aspnetcore_env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var connectionString = string.Equals(aspnetcore_env, "Development") ? configuration.GetSection("Data")["ConnectionString_Debug"] : configuration.GetSection("Data")["ConnectionString_Debug"];
            var selectedDataProvider = DbContextFactory.GetCurrentDataProvider(dataProviderConfig);
            selectedDataProvider.RegisterDbContext(services, connectionString);
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            { 
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationContext>();//.AddDefaultTokenProviders();

            return services;
        }
    }
}
