using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace One.Data.Migrations
{
    public partial class Avatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Portrait",
                table: "AspNetUsers",
                newName: "Avatar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "AspNetUsers",
                newName: "Portrait");
        }
    }
}
