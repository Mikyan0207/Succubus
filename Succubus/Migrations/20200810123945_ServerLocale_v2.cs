using Microsoft.EntityFrameworkCore.Migrations;

namespace Succubus.Migrations
{
    public partial class ServerLocale_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "Servers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locale",
                table: "Servers");
        }
    }
}
