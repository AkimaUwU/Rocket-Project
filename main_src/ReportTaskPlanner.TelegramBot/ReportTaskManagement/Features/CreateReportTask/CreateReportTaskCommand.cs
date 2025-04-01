using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Service.Facade;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;

public sealed record CreateReportTaskCommand(string TextPayload, long MessageId)
    : ICommand<ReportTask>;

public sealed class CreateReportTaskCommandHandler(
    IReportTaskRepository repository,
    CreateReportTaskContext context,
    ITimeRecognitionFacade facade
) : ICommandHandler<CreateReportTaskCommand, ReportTask>
{
    private readonly IReportTaskRepository _repository = repository;
    private readonly CreateReportTaskContext _context = context;
    private readonly ITimeRecognitionFacade _facade = facade;

    public async Task<Result<ReportTask>> Handle(CreateReportTaskCommand command)
    {
        ApplicationTime currentTime = _context.CurrentTime.Value;
        string inputText = command.TextPayload;
        Result<ReportTaskSchedule> schedule = await _facade.CreateTaskSchedule(
            inputText,
            currentTime
        );
        if (schedule.IsFailure)
            return schedule.Error;
        ReportTask createdTask = new(command.MessageId, command.TextPayload, schedule);
        await _repository.Save(createdTask);
        return createdTask;
    }
}
