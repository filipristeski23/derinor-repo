using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Derinor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanFieldAndLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Users");
        }
    }
}
