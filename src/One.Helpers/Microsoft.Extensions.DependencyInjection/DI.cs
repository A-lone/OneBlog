using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class DI
    {
        public static IServiceProvider ServiceProvider
        {
            get; set;
        }
    }

    public static class DIExtensions
    {
        public static IServiceCollection AddMvcDI(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder UseMvcDI(this IApplicationBuilder builder)
        {
            DI.ServiceProvider = builder.ApplicationServices;
            return builder;
        }
    }
}
