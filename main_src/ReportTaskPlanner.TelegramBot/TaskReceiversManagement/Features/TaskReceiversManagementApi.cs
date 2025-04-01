using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.GetReceivers;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features;

public sealed class TaskReceiversManagementApi(
    ICommandHandler<RegisterReceiverCommand, TaskReceiver> registerReceiver,
    ICommandHandler<RemoveReceiverCommand, bool> removeReceiver,
    IQueryHandler<GetReceiversQuery, Option<IEnumerable<TaskReceiver>>> getReceivers
)
{
    private readonly ICommandHandler<RegisterReceiverCommand, TaskReceiver> _registerReceiver =
        registerReceiver;

    private readonly ICommandHandler<RemoveReceiverCommand, bool> _removeReceiver = removeReceiver;

    private readonly IQueryHandler<
        GetReceiversQuery,
        Option<IEnumerable<TaskReceiver>>
    > _getReceivers = getReceivers;

    public async Task<Result<TaskReceiver>> RegisterReceiver(long id) =>
        await _registerReceiver.Handle(new RegisterReceiverCommand(id));

    public async Task<bool> RemoveReceiver(long id) =>
        await _removeReceiver.Handle(new RemoveReceiverCommand(id));

    public async Task<Option<IEnumerable<TaskReceiver>>> GetReceivers() =>
        await _getReceivers.Handle(new GetReceiversQuery());
}
