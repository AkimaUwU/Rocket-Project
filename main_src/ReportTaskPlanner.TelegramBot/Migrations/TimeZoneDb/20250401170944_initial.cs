using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportTaskPlanner.TelegramBot.Migrations.TimeZoneDb
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Time_Zone_Db_Options",
                columns: table => new
                {
                    Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token", x => x.Token);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Time_Zone_Db_Options");
        }
    }
}
