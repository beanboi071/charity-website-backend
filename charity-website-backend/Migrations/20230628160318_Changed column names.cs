using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace charity_website_backend.Migrations
{
    /// <inheritdoc />
    public partial class Changedcolumnnames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetAmount",
                table: "Projects",
                newName: "Target_Amount");

            migrationBuilder.RenameColumn(
                name: "NGOId",
                table: "Projects",
                newName: "NGO_Id");

            migrationBuilder.RenameColumn(
                name: "ImageBase64",
                table: "Projects",
                newName: "Image_Path");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Projects",
                newName: "Created_Date_And_Time");

            migrationBuilder.RenameColumn(
                name: "AmountRaised",
                table: "Projects",
                newName: "Amount_Raised");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "NGOs",
                newName: "Password_Hash");

            migrationBuilder.RenameColumn(
                name: "ImageBase64",
                table: "NGOs",
                newName: "Image_Path");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Donors",
                newName: "Password_Hash");

            migrationBuilder.RenameColumn(
                name: "ImageBase64",
                table: "Donors",
                newName: "Image_Path");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Donations",
                newName: "Project_Id");

            migrationBuilder.RenameColumn(
                name: "DonorId",
                table: "Donations",
                newName: "Donor_Id");

            migrationBuilder.RenameColumn(
                name: "DateAndTime",
                table: "Donations",
                newName: "Date_And_Time");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Admin",
                newName: "Password_Hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Target_Amount",
                table: "Projects",
                newName: "TargetAmount");

            migrationBuilder.RenameColumn(
                name: "NGO_Id",
                table: "Projects",
                newName: "NGOId");

            migrationBuilder.RenameColumn(
                name: "Image_Path",
                table: "Projects",
                newName: "ImageBase64");

            migrationBuilder.RenameColumn(
                name: "Created_Date_And_Time",
                table: "Projects",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Amount_Raised",
                table: "Projects",
                newName: "AmountRaised");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "NGOs",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Image_Path",
                table: "NGOs",
                newName: "ImageBase64");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "Donors",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Image_Path",
                table: "Donors",
                newName: "ImageBase64");

            migrationBuilder.RenameColumn(
                name: "Project_Id",
                table: "Donations",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "Donor_Id",
                table: "Donations",
                newName: "DonorId");

            migrationBuilder.RenameColumn(
                name: "Date_And_Time",
                table: "Donations",
                newName: "DateAndTime");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "Admin",
                newName: "PasswordHash");
        }
    }
}
