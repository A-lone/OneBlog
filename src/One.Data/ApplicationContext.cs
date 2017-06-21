using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace One.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfigurationRoot config) : base(options)
        {
            _config = config;
        }

        private IConfigurationRoot _config;

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

            var typesToRegister = typeof(ApplicationContext).GetTypeInfo().Assembly.GetTypes()
                            .Where(type => !string.IsNullOrEmpty(type.Namespace))
                            .Where(type => type.FullName.Contains("One.Data.Mapping"));
            foreach (var type in typesToRegister)
            {
                IEntityMapping mapping = (IEntityMapping)Activator.CreateInstance(type);
                mapping.Execute(builder);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config["OneDb:ConnectionString"]);
            base.OnConfiguring(optionsBuilder);
        }

    }
}