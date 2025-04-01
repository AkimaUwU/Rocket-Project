using Microsoft.Data.Sqlite;

namespace ReportTaskPlanner.TelegramBot.Shared.Data;

public static class ConnectionStringFactory
{
    public static SqliteConnection TimeZoneDbConnection => new SqliteConnection();
    public static SqliteConnection ApplicationTimeDbConnection => new SqliteConnection();
    public static SqliteConnection ReportTasksDbConnection => new SqliteConnection();
    public static SqliteConnection TaskReceiversDbConnection => new SqliteConnection();
}
