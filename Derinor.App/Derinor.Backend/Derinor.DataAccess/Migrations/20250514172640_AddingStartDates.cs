using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Derinor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingStartDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingDate",
                table: "ProjectBranches");

            migrationBuilder.AddColumn<string>(
                name: "GithubAccessToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartingDate",
                table: "ProjectBranches",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubAccessToken",
                table: "Users");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartingDate",
                table: "ProjectBranches",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndingDate",
                table: "ProjectBranches",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
