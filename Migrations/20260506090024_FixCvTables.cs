using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentApp.API.Migrations
{
    /// <inheritdoc />
    public partial class FixCvTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CvAnalyses_CvUpload_CvUploadId",
                table: "CvAnalyses");

            migrationBuilder.DropForeignKey(
                name: "FK_CvUpload_Users_UserId",
                table: "CvUpload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CvUpload",
                table: "CvUpload");

            migrationBuilder.RenameTable(
                name: "CvUpload",
                newName: "CvUploads");

            migrationBuilder.RenameIndex(
                name: "IX_CvUpload_UserId",
                table: "CvUploads",
                newName: "IX_CvUploads_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CvUploads",
                table: "CvUploads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CvAnalyses_CvUploads_CvUploadId",
                table: "CvAnalyses",
                column: "CvUploadId",
                principalTable: "CvUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CvUploads_Users_UserId",
                table: "CvUploads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CvAnalyses_CvUploads_CvUploadId",
                table: "CvAnalyses");

            migrationBuilder.DropForeignKey(
                name: "FK_CvUploads_Users_UserId",
                table: "CvUploads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CvUploads",
                table: "CvUploads");

            migrationBuilder.RenameTable(
                name: "CvUploads",
                newName: "CvUpload");

            migrationBuilder.RenameIndex(
                name: "IX_CvUploads_UserId",
                table: "CvUpload",
                newName: "IX_CvUpload_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CvUpload",
                table: "CvUpload",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CvAnalyses_CvUpload_CvUploadId",
                table: "CvAnalyses",
                column: "CvUploadId",
                principalTable: "CvUpload",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CvUpload_Users_UserId",
                table: "CvUpload",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}