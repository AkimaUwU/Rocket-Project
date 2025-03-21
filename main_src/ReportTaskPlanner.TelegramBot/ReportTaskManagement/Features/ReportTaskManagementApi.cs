using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features;

public sealed class ReportTaskManagementApi(
    ICommandHandler<CreateReportTaskCommand, ReportTask> handler
)
{
    private readonly ICommandHandler<CreateReportTaskCommand, ReportTask> _handler = handler;

    public async Task<Result<ReportTask>> CreateNewTask(string textPayload, long id)
    {
        CreateReportTaskCommand command = new(textPayload, id);
        return await _handler.Handle(command);
    }
}
