using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace One.Data.Migrations
{
    public partial class add_count : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Teaser",
                table: "Comments");

            migrationBuilder.AddColumn<long>(
                name: "Count",
                table: "Posts",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Teaser",
                table: "Comments",
                nullable: true);
        }
    }
}
