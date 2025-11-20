using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisionShare.Migrations
{
    /// <inheritdoc />
    public partial class SyncModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ideas",
                columns: new[] { "IdeaId", "Author", "DatePosted", "Description", "FeatureImagePath", "Title", "UserId" },
                values: new object[] { 1, "John Doe", new DateTime(2025, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is the description of the first idea.", "/images/idea1.png", "First Idea", "user1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ideas",
                keyColumn: "IdeaId",
                keyValue: 1);
        }
    }
}
