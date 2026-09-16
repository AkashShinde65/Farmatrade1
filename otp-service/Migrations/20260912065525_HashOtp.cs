using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmaTrade_OTP_Service.Migrations
{
    /// <inheritdoc />
    public partial class HashOtp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Otp",
                table: "OtpRecords",
                newName: "OtpHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OtpHash",
                table: "OtpRecords",
                newName: "Otp");
        }
    }
}
