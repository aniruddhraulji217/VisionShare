using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisionShare.Migrations
{
    /// <inheritdoc />
    public partial class AddViewCountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateVoted",
                table: "IdeaUpvotes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpvoteId",
                table: "IdeaUpvotes",
                newName: "IdeaUpvoteId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Ideas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Ideas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Ideas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Ideas",
                keyColumn: "IdeaId",
                keyValue: 1,
                column: "ViewCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Ideas",
                keyColumn: "IdeaId",
                keyValue: 2,
                column: "ViewCount",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Ideas");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "IdeaUpvotes",
                newName: "DateVoted");

            migrationBuilder.RenameColumn(
                name: "IdeaUpvoteId",
                table: "IdeaUpvotes",
                newName: "UpvoteId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Ideas",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Ideas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
