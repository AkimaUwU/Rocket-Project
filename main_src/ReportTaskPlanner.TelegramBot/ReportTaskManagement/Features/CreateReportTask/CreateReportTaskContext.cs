using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.TimeRecognition;
using ReportTaskPlanner.TelegramBot.Shared.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;

public sealed class CreateReportTaskContext : SharedContext
{
    public Option<ApplicationTime> CurrentTime { get; set; } = Option<ApplicationTime>.None();
    public Option<long> RequestedUnixTime { get; set; } = Option<long>.None();
    public Option<TimeRecognitionResult> TimeRecognitionResult { get; set; } =
        Option<TimeRecognitionResult>.None();
}
