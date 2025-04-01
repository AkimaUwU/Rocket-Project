using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportTaskPlanner.TelegramBot.Migrations.ReportTaskDb
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Report_Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    task_text = table.Column<string>(type: "TEXT", nullable: false),
                    task_schedule_is_periodic = table.Column<bool>(type: "INTEGER", nullable: false),
                    task_schedule_created_unix = table.Column<long>(type: "INTEGER", nullable: false),
                    task_schedule_notify_time = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("task_id", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Report_Tasks");
        }
    }
}
