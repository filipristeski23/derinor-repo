using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Derinor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubUsername",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubUsername",
                table: "Users");
        }
    }
}
