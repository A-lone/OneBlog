using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace One.Mock.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataEventRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEventRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cookie = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SitePaths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cookie = table.Column<string>(nullable: true),
                    DLL = table.Column<string>(nullable: true),
                    Expression = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Query = table.Column<string>(nullable: true),
                    RequestEnabled = table.Column<bool>(nullable: false),
                    SitesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SitePaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SitePaths_Sites_SitesId",
                        column: x => x.SitesId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SitePaths_SitesId",
                table: "SitePaths",
                column: "SitesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataEventRecords");

            migrationBuilder.DropTable(
                name: "SitePaths");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
