using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;

public sealed class UpdateApplicationTimeSharedContext : SharedContext
{
    public Option<ApplicationTime> Deserialized { get; set; } = Option<ApplicationTime>.None();
    public Option<string> TimeZoneDbJson { get; set; } = Option<string>.None();
}
