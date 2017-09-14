using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace OneBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IDbContextFactory _factory;
        public ApplicationDbContext(IDbContextFactory factory, DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _factory = factory;
        }


        /// <summary>
        /// 文章
        /// </summary>
        public DbSet<Posts> Posts { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<StoreCategories> StoreCategories { get; set; }

        public DbSet<StoreApp> StoreApp { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public DbSet<Categories> Categories { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public DbSet<PostsInCategories> PostsInCategories { get; set; }

        public DbSet<TagsInPosts> TagsInPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Override the name of the table because of a RC2 change
            builder.Entity<Posts>().ToTable("Posts");
            builder.Entity<Tags>().ToTable("Tags");
            builder.Entity<Categories>().ToTable("Categories");
            builder.Entity<PostsInCategories>().ToTable("PostsInCategories");
            builder.Entity<TagsInPosts>().ToTable("TagsInPosts");
            builder.Entity<Comments>().ToTable("Comments");
            builder.Entity<StoreApp>().ToTable("StoreApp");
            builder.Entity<StoreCategories>().ToTable("StoreCategories");

            var typesToRegister = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetTypes()
                            .Where(type => !string.IsNullOrEmpty(type.Namespace))
                            .Where(type => type.FullName.Contains("OneBlog.Data.Mapping"));
            foreach (var type in typesToRegister)
            {
                IEntityMapping mapping = (IEntityMapping)Activator.CreateInstance(type);
                mapping.Execute(builder);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _factory.Configuring(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var aspnetcore_env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //    //var connectionString = _config[string.Equals(aspnetcore_env, "Development") ? "OneDb:ConnectionString_Test" : "OneDb:ConnectionString"];
        //    optionsBuilder.UseSqlServer("Server=.;Database=OneBlog;User ID=sa;Password=abcd1234!");
        //    base.OnConfiguring(optionsBuilder);
        //}

    }
}