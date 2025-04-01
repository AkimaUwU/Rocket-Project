using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones;

public sealed class ListTimeZonesSharedContext
{
    public Option<TimeZoneDbOptions> Options { get; set; } = Option<TimeZoneDbOptions>.None();
    public Option<Error> Error { get; set; } = Option<Error>.None();
    public Option<string> TimeZoneDbResponseJson { get; set; } = Option<string>.None();

    public Option<ApplicationTime[]> DeserializedTimeZones { get; set; } =
        Option<ApplicationTime[]>.None();
}
