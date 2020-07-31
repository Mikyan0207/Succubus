using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Succubus.Database.Migrations
{
    public partial class UserImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cosplayers",
                columns: table => new
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
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cosplayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    ServerId = table.Column<ulong>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false),
                    Experience = table.Column<ulong>(nullable: false),
                    Level = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Aliases = table.Column<string>(nullable: true),
                    Size = table.Column<uint>(nullable: false),
                    SetPreview = table.Column<string>(nullable: false),
                    CosplayerId = table.Column<Guid>(nullable: true),
                    YabaiLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Cosplayers_CosplayerId",
                        column: x => x.CosplayerId,
                        principalTable: "Cosplayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    CosplayerId = table.Column<Guid>(nullable: true),
                    SetId = table.Column<Guid>(nullable: true),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Cosplayers_CosplayerId",
                        column: x => x.CosplayerId,
                        principalTable: "Cosplayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Images_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ImageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CosplayerId",
                table: "Images",
                column: "CosplayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_SetId",
                table: "Images",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_CosplayerId",
                table: "Sets",
                column: "CosplayerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_ImageId",
                table: "UserImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_UserId",
                table: "UserImages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Cosplayers");
        }
    }
}
