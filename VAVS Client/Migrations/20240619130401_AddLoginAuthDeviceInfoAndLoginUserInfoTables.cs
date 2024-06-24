using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAVS_Client.Migrations
{
    public partial class AddLoginAuthDeviceInfoAndLoginUserInfoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_DeviceInfo",
                columns: table => new
                {
                    DeviceInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublicIpAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RegistrationCount = table.Column<int>(type: "int", nullable: false),
                    ResendCodeTime = table.Column<int>(type: "int", nullable: false),
                    ReRegistrationTime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReResendCodeTime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OTP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DeviceInfo", x => x.DeviceInfoId);
                });

            migrationBuilder.CreateTable(
                name: "TB_LoginAuth",
                columns: table => new
                {
                    LoginAuthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nrc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResendOTPCount = table.Column<int>(type: "int", nullable: false),
                    ReResendCodeTime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OTP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LoginAuth", x => x.LoginAuthId);
                });

            migrationBuilder.CreateTable(
                name: "TB_LoginUserInfo",
                columns: table => new
                {
                    LoginUserInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VehicleNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VehicleBrand = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BuildType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CountryOfMade = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ModelYear = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EnginePower = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StandardValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContractValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TaxAmount = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LoggedInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RememberMe = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LoginUserInfo", x => x.LoginUserInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_DeviceInfo");

            migrationBuilder.DropTable(
                name: "TB_LoginAuth");

            migrationBuilder.DropTable(
                name: "TB_LoginUserInfo");
        }
    }
}
