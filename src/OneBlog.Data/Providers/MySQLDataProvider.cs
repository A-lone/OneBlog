using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OneBlog.Configuration;

namespace OneBlog.Data.Providers
{
    public class MySQLDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.MySQL;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options => 
                options.UseMySql(connectionString));

            return services;
        }

        public ApplicationContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseMySql(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}