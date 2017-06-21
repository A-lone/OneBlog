using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneBlog.Data.Migrations
{
    public partial class addcomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCommentsEnabled",
                table: "Posts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpam",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Teaser",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCommentsEnabled",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsSpam",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Teaser",
                table: "Comments");
        }
    }
}
