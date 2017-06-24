using Microsoft.Extensions.Options;
using OneBlog.Configuration;
using OneBlog.Data.Providers;
using OneBlog.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace OneBlog.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private DataConfiguration DataConfiguration { get; }
        private string ConnectionString { get; set; }

        public DbContextFactory(IOptions<DataConfiguration> dataOptions)
        {
            DataConfiguration = dataOptions.Value;
            var aspnetcore_env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            ConnectionString = string.Equals(aspnetcore_env, "Development") ? DataConfiguration.ConnectionString_Debug : DataConfiguration.ConnectionString;
        }

        public ApplicationContext Create()
        {
            var dataProviderConfig = DataConfiguration.Provider.ToString();
            var selectedDataProvider = GetCurrentDataProvider(dataProviderConfig);
            return selectedDataProvider.CreateDbContext(ConnectionString);
        }

        private static IDataProvider GetCurrentDataProvider(string dataProvider)
        {
            var currentAssembly = typeof(DbContextFactory).GetTypeInfo().Assembly;
            var allDataProviders = currentAssembly.GetTypes<IDataProvider>();
            var selectedDataProvider = allDataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProvider);
            return selectedDataProvider;
        }
    }



    public interface IDbContextFactory
    {
        ApplicationContext Create();
    }
}
