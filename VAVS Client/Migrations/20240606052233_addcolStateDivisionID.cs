using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAVS_Client.Migrations
{
    public partial class addcolStateDivisionID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateDivisionID",
                table: "TB_Township",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "StateDivisionID",
                table: "TB_Township");
        }
    }
}
