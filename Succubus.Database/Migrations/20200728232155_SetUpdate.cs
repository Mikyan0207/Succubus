using Microsoft.EntityFrameworkCore.Migrations;

namespace Succubus.Database.Migrations
{
    public partial class SetUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aliases",
                table: "Sets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aliases",
                table: "Sets");
        }
    }
}
