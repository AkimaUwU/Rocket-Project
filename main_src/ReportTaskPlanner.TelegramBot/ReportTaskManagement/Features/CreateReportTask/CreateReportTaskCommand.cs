using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.DateOffsetCalculation;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.DateConverting.TimeRecognition;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;

public sealed record CreateReportTaskCommand(string TextPayload, long messageId)
    : ICommand<ReportTask>;

public sealed class CreateReportTaskCommandHandler(
    ReportTaskRepository repository,
    CreateReportTaskContext context,
    IDateOffsetCalculation calculation
) : ICommandHandler<CreateReportTaskCommand, ReportTask>
{
    private readonly IDateOffsetCalculation _calculation = calculation;
    private readonly ReportTaskRepository _repository = repository;
    private readonly CreateReportTaskContext _context = context;

    public async Task<Result<ReportTask>> Handle(CreateReportTaskCommand command)
    {
        ApplicationTime currentTime = _context.CurrentTime.Value;
        TimeRecognitionResult timeRecognition = _context.TimeRecognitionResult;
        Option<DateOffsetResult> offset = _calculation.Convert(command.TextPayload, currentTime);
        if (!offset.HasValue)
            return new Error("Не удается распознать дату уведомления.");

        WhenToNotifyTimeCalculator calculator = new WhenToNotifyTimeCalculator(
            offset,
            timeRecognition
        );

        long calculatedSeconds = calculator.CalculateWhenToFireTime();
        ReportTaskSchedule schedule = new ReportTaskSchedule(
            currentTime.TimeStamp,
            calculatedSeconds
        );

        ReportTask task = new ReportTask(command.messageId, command.TextPayload, schedule);
        await _repository.Save(task);
        return task;
    }
}
