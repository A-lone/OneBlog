using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneBlog.Data.Migrations
{
    public partial class store : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppName = table.Column<string>(nullable: true),
                    CategoriesId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PDB = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreApp_StoreCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "StoreCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreApp_CategoriesId",
                table: "StoreApp",
                column: "CategoriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreApp");

            migrationBuilder.DropTable(
                name: "StoreCategories");
        }
    }
}
