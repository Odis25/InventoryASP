using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryAppData.Migrations
{
    public partial class Yearcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Devices");
        }
    }
}
