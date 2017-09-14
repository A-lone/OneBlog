using System;
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }

        public void Configuring(DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}