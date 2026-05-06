using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentApp.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCvAnalysisForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CvAnalyses_CvUploads_CvUploadId",
                table: "CvAnalyses");

            migrationBuilder.DropIndex(
                name: "IX_CvAnalyses_CvUploadId",
                table: "CvAnalyses");

            migrationBuilder.DropColumn(
                name: "CvUploadId",
                table: "CvAnalyses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CvUploadId",
                table: "CvAnalyses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CvAnalyses_CvUploadId",
                table: "CvAnalyses",
                column: "CvUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_CvAnalyses_CvUploads_CvUploadId",
                table: "CvAnalyses",
                column: "CvUploadId",
                principalTable: "CvUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}