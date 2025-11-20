using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisionShare.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ideas",
                columns: new[] { "IdeaId", "Author", "DatePosted", "Description", "FeatureImagePath", "Title", "UserId" },
                values: new object[] { 2, "Jane Smith", new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "This is the description of the second idea.", "/images/idea2.png", "Second Idea", "user2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ideas",
                keyColumn: "IdeaId",
                keyValue: 2);
        }
    }
}
