using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketPlaner.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.TelegramId);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerTelegramId = table.Column<long>(type: "INTEGER", nullable: false),
                    FireDate_FireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message_Message = table.Column<string>(type: "TEXT", nullable: false),
                    Title_Title = table.Column<string>(type: "TEXT", nullable: false),
                    Type_Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_OwnerTelegramId",
                        column: x => x.OwnerTelegramId,
                        principalTable: "Users",
                        principalColumn: "TelegramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    BelongsToId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.ChatId);
                    table.ForeignKey(
                        name: "FK_Destinations_Tasks_BelongsToId",
                        column: x => x.BelongsToId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_BelongsToId",
                table: "Destinations",
                column: "BelongsToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerTelegramId",
                table: "Tasks",
                column: "OwnerTelegramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
