using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using One.Mock.Data;

namespace One.Mock.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20170324090639_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("One.Mock.Data.DataEventRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.ToTable("DataEventRecords");
                });

            modelBuilder.Entity("One.Mock.Data.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cookie");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("One.Mock.Data.SitePath", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cookie");

                    b.Property<string>("DLL");

                    b.Property<string>("Expression");

                    b.Property<string>("Json");

                    b.Property<string>("Method");

                    b.Property<string>("Path");

                    b.Property<string>("Query");

                    b.Property<bool>("RequestEnabled");

                    b.Property<Guid?>("SitesId");

                    b.HasKey("Id");

                    b.HasIndex("SitesId");

                    b.ToTable("SitePaths");
                });

            modelBuilder.Entity("One.Mock.Data.SitePath", b =>
                {
                    b.HasOne("One.Mock.Data.Site", "Sites")
                        .WithMany("SitePaths")
                        .HasForeignKey("SitesId");
                });
        }
    }
}
