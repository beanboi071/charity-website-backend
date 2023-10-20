using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity_website_backend.Migrations
{
    /// <inheritdoc />
    public partial class expirytimecolumnaddedinOTPtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryTime",
                table: "OTPs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryTime",
                table: "OTPs");
        }
    }
}
