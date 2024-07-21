using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAVS_Client.Migrations
{
    public partial class CreatePaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Payment",
                columns: table => new
                {
                    PaymentPkid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IncomeYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentHubRefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankTransactionRefNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    PersonalPkid = table.Column<int>(type: "int", nullable: false),
                    TaxValidationPkid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Payment", x => x.PaymentPkid);
                    table.ForeignKey(
                        name: "FK_TB_Payment_TB_PersonalDetail_PersonalPkid",
                        column: x => x.PersonalPkid,
                        principalTable: "TB_PersonalDetail",
                        principalColumn: "PersonalPkid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_Payment_TB_TaxValidation_TaxValidationPkid",
                        column: x => x.TaxValidationPkid,
                        principalTable: "TB_TaxValidation",
                        principalColumn: "TaxValidationPkid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Payment_PersonalPkid",
                table: "TB_Payment",
                column: "PersonalPkid");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Payment_TaxValidationPkid",
                table: "TB_Payment",
                column: "TaxValidationPkid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Payment");
        }
    }
}
