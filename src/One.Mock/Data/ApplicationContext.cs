using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Mock.Data
{
    // >dotnet ef migration add testMigration
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        public DbSet<SitePath> SitePaths { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<DataEventRecord> DataEventRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DataEventRecord>().HasKey(m => m.Id);
            builder.Entity<Site>().HasKey(m => m.Id);
            builder.Entity<SitePath>().HasKey(m => m.Id);
            builder.Entity<SitePath>().HasOne(x => x.Sites).WithMany(x => x.SitePaths);

            base.OnModelCreating(builder);
        }
    }
}
