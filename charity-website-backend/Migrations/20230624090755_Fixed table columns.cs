using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity_website_backend.Migrations
{
    /// <inheritdoc />
    public partial class Fixedtablecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stutus",
                table: "Projects",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PaswordHash",
                table: "NGOs",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "NGOs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "NGOs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "NGOs");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Projects",
                newName: "Stutus");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "NGOs",
                newName: "PaswordHash");

            migrationBuilder.AlterColumn<int>(
                name: "Username",
                table: "NGOs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
