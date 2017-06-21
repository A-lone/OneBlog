using OneBlog.AspNetCore.TimedJob;
using OneBlog.AspNetCore.TimedJob.EntityFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityFrameworkDynamicTimedJobServiceBuilderExtensions
    {
        public static IServiceCollection AddEntityFrameworkDynamicTimedJob<TContext>(this IServiceCollection self)
            where TContext : ITimedJobContext
        {
            return self.AddScoped<IDynamicTimedJobProvider, EntityFrameworkDynamicTimedJobProvider<TContext>>();
        }
    }
}
