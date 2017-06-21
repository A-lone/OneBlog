using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace One.Data.Migrations
{
    public partial class add_site_url : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteUrl",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteUrl",
                table: "AspNetUsers");
        }
    }
}
