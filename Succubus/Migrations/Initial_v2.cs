using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Succubus.Migrations
{
    public partial class Initial_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Colors",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Red = table.Column<byte>(nullable: false),
                    Green = table.Column<byte>(nullable: false),
                    Blue = table.Column<byte>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Colors", x => x.Id); });

            migrationBuilder.CreateTable(
                "Cosplayers",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Aliases = table.Column<string>(nullable: false),
                    ProfilePicture = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Booth = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Cosplayers", x => x.Id); });

            migrationBuilder.CreateTable(
                "Servers",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    ServerId = table.Column<ulong>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Servers", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false),
                    Experience = table.Column<ulong>(nullable: false),
                    Level = table.Column<ulong>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Sets",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Aliases = table.Column<string>(nullable: true),
                    Size = table.Column<uint>(nullable: false),
                    SetPreview = table.Column<string>(nullable: false),
                    FolderName = table.Column<string>(nullable: true),
                    FilePrefix = table.Column<string>(nullable: true),
                    CosplayerId = table.Column<Guid>(nullable: true),
                    YabaiLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        "FK_Sets_Cosplayers_CosplayerId",
                        x => x.CosplayerId,
                        "Cosplayers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Sets_CosplayerId",
                "Sets",
                "CosplayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Colors");

            migrationBuilder.DropTable(
                "Servers");

            migrationBuilder.DropTable(
                "Sets");

            migrationBuilder.DropTable(
                "Users");

            migrationBuilder.DropTable(
                "Cosplayers");
        }
    }
}