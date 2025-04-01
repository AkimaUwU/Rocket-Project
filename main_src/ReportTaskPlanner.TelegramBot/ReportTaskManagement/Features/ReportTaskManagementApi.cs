using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.CreateReportTask;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.GetPendingTasks;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.UpdateTasks;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features;

public sealed class ReportTaskManagementApi(
    ICommandHandler<CreateReportTaskCommand, ReportTask> createHandler,
    ICommandHandler<UpdateTasksCommand, int> updateManyHandler,
    ICommandHandler<RemovePendingTasksCommand, int> removeManyHandler,
    IQueryHandler<GetPendingTasksQuery, Option<IEnumerable<ReportTask>>> getPendingTasks
)
{
    private readonly ICommandHandler<CreateReportTaskCommand, ReportTask> _createHandler =
        createHandler;

    private readonly ICommandHandler<UpdateTasksCommand, int> _updateManyHandler =
        updateManyHandler;

    private readonly ICommandHandler<RemovePendingTasksCommand, int> _removeManyHandler =
        removeManyHandler;

    private readonly IQueryHandler<
        GetPendingTasksQuery,
        Option<IEnumerable<ReportTask>>
    > _getPendingTasks = getPendingTasks;

    public async Task<Result<ReportTask>> CreateNewTask(string textPayload, long id) =>
        await _createHandler.Handle(new CreateReportTaskCommand(textPayload, id));

    public async Task<Result<int>> UpdateMany(ReportTask[] tasks) =>
        await _updateManyHandler.Handle(new UpdateTasksCommand(tasks));

    public async Task<Result<int>> RemoveMany(ReportTask[] tasks) =>
        await _removeManyHandler.Handle(new RemovePendingTasksCommand(tasks));

    public async Task<Option<IEnumerable<ReportTask>>> GetPendingTasks(ApplicationTime time) =>
        await _getPendingTasks.Handle(new GetPendingTasksQuery(time));
}
