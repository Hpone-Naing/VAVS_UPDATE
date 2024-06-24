using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAVS_Client.Migrations
{
    public partial class Addnewcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GIR",
                table: "TB_PersonalDetail",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GIR",
                table: "TB_PersonalDetail");
        }
    }
}
