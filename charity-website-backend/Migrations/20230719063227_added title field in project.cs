using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity_website_backend.Migrations
{
    /// <inheritdoc />
    public partial class addedtitlefieldinproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Projects");
        }
    }
}
