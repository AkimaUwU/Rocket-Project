using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;

public sealed class CreateReportTaskContext : SharedContext
{
    public Option<ApplicationTime> CurrentTime { get; set; } = Option<ApplicationTime>.None();
}
