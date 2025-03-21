using LiteDB;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;

public static class TimeZoneDbRepositoryConstants
{
    public const string CollectionName = "Time_Zone_Configuration";

    public static readonly string ConnectionString =
        "Filename=TimeZoneConfiguration.db;Connection=shared";

    public static void RegisterBsonMapping() =>
        BsonMapper.Global.Entity<TimeZoneDbOptions>().Field(t => t.Token, "time_zone_token");
}
