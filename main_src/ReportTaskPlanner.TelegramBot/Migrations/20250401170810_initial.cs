using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportTaskPlanner.TelegramBot.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Application_Time",
                columns: table => new
                {
                    ZoneName = table.Column<string>(type: "TEXT", nullable: false),
                    display_name = table.Column<string>(type: "TEXT", nullable: false),
                    time_stamp = table.Column<long>(type: "INTEGER", nullable: false),
                    date_time = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("time_zone_name", x => x.ZoneName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Application_Time");
        }
    }
}
