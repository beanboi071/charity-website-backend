using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity_website_backend.Migrations
{
    /// <inheritdoc />
    public partial class NGOentitychanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "NGOs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Website_Link",
                table: "NGOs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "NGOs");

            migrationBuilder.DropColumn(
                name: "Website_Link",
                table: "NGOs");
        }
    }
}
