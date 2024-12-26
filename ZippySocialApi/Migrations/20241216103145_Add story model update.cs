using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZippySocialApi.Migrations
{
    /// <inheritdoc />
    public partial class Addstorymodelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoryImg",
                table: "Story");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StoryImg",
                table: "Story",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
