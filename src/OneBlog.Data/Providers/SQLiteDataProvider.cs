using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OneBlog.Configuration;

namespace OneBlog.Data.Providers
{
    public class SQLiteDataProvider : IDataProvider
    {
        public DataProvider Provider { get; } = DataProvider.SQLite;

        public IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }

        public ApplicationContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}