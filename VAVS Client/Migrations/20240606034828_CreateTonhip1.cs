using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VAVS_Client.Migrations
{
    public partial class CreateTonhip1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Township1",
                columns: table => new
                {
                    TownshipPkid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TownshipCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TownshipName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DistrictCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    StateDivisionID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Township1", x => x.TownshipPkid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Township1");
        }
    }
}
