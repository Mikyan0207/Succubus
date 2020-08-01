using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Succubus.Database.Migrations
{
    public partial class YtChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YoutubeChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ChannelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordChannels",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    ServerId = table.Column<Guid>(nullable: true),
                    NotificationActivated = table.Column<bool>(nullable: false),
                    YoutubeChannelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordChannels_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannels_YoutubeChannels_YoutubeChannelId",
                        column: x => x.YoutubeChannelId,
                        principalTable: "YoutubeChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannels_ServerId",
                table: "DiscordChannels",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannels_YoutubeChannelId",
                table: "DiscordChannels",
                column: "YoutubeChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordChannels");

            migrationBuilder.DropTable(
                name: "YoutubeChannels");
        }
    }
}
