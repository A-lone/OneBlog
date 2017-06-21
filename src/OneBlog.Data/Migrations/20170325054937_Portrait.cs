using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace One.Data.Migrations
{
    public partial class Portrait : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Portrait",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Portrait",
                table: "AspNetUsers");
        }
    }
}
