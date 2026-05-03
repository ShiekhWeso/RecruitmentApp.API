using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiresAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiersAt",
                table: "PasswordResetTokens",
                newName: "ExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "PasswordResetTokens",
                newName: "ExpiersAt");
        }
    }
}
