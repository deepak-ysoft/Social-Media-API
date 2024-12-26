using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZippySocialApi.Migrations
{
    /// <inheritdoc />
    public partial class addstory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Story_Users_UserId",
                table: "Story");

            migrationBuilder.DropIndex(
                name: "IX_Story_UserId",
                table: "Story");

            migrationBuilder.AddColumn<string>(
                name: "AboutYou",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Story",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AboutStory",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutYou",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AboutStory",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Story");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Story_UserId",
                table: "Story",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Story_Users_UserId",
                table: "Story",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
