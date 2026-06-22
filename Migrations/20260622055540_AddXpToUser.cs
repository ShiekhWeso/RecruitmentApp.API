using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddXpToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XP",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XP",
                table: "Users");
        }
    }
}
