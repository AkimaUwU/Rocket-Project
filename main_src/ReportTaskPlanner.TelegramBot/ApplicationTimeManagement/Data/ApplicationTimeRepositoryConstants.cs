using LiteDB;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;

public static class ApplicationTimeRepositoryConstants
{
    public const string CollectionName = "Application_Time";

    public static readonly string ConnectionString =
        "Filename=ApplicationTime.db;Connection=Shared";

    public static void RegisterBsonMapping() =>
        BsonMapper
            .Global.Entity<ApplicationTime>()
            .Field(t => t.DisplayName, "time_display_name")
            .Field(t => t.DateTime, "time_datetime")
            .Field(t => t.TimeStamp, "time_timestamp")
            .Field(t => t.ZoneName, "time_zone_name");
}
