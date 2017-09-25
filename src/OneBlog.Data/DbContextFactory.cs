using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OneBlog.Configuration;
using OneBlog.Data.Providers;
using OneBlog.Helpers;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OneBlog.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>, IDbContextFactory
    {

        private DataSettings DataConfiguration { get; }
        private string ConnectionString { get; set; }

        public DbContextFactory(IOptions<DataSettings> dataOptions)
        {
            DataConfiguration = dataOptions.Value;
            ConnectionString = DataConfiguration.ConnectionString;
        }


        public void Configuring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataProviderConfig = DataConfiguration.Provider.ToString();
            var selectedDataProvider = GetCurrentDataProvider(dataProviderConfig);
            selectedDataProvider.Configuring(optionsBuilder, ConnectionString);
        }

        public static IDataProvider GetCurrentDataProvider(string dataProvider)
        {
            var currentAssembly = typeof(DbContextFactory).GetTypeInfo().Assembly;
            var allDataProviders = currentAssembly.GetTypes<IDataProvider>();
            var selectedDataProvider = allDataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProvider);
            return selectedDataProvider;
        }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            return new ApplicationDbContext(this, builder.Options);
        }
    }

    public interface IDbContextFactory
    {
        void Configuring(DbContextOptionsBuilder optionsBuilder);
    }
}
