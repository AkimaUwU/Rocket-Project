using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.GetPendingTasks;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.GetReceivers;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;
using Telegram.Bot;

namespace ReportTaskPlanner.TelegramBot.TasksNotificationManagement;

public sealed class TasksNotificaitonManager(
    IQueryHandler<GetPendingTasksQuery, Option<IEnumerable<ReportTask>>> getPendingTasks,
    IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>> getAppTime,
    IQueryHandler<GetReceiversQuery, Option<IEnumerable<TaskReceiver>>> getReceivers,
    ICommandHandler<RemovePendingTasksCommand, bool> removePendingTasks,
    TelegramBotClient client,
    Serilog.ILogger logger
)
{
    private readonly IQueryHandler<
        GetPendingTasksQuery,
        Option<IEnumerable<ReportTask>>
    > _getPendingTasks = getPendingTasks;

    private readonly IQueryHandler<
        GetUpdatedApplicationTimeQuery,
        Option<ApplicationTime>
    > _getAppTime = getAppTime;

    private readonly IQueryHandler<
        GetReceiversQuery,
        Option<IEnumerable<TaskReceiver>>
    > _getReceivers = getReceivers;

    private readonly ICommandHandler<RemovePendingTasksCommand, bool> _removePendingTasks =
        removePendingTasks;
    private readonly TelegramBotClient _client = client;

    private readonly Serilog.ILogger _logger = logger;

    public async Task ManagePendingTasks()
    {
        _logger.Information("Managing pending tasks...");
        Option<ApplicationTime> appTime = await _getAppTime.Handle(new());
        Option<IEnumerable<ReportTask>> tasks = await _getPendingTasks.Handle(new());
        Option<IEnumerable<TaskReceiver>> receivers = await _getReceivers.Handle(new());
        if (!appTime.HasValue)
        {
            _logger.Warning("Managing pending tasks stopped. Application time is not configured.");
            return;
        }
        if (!tasks.HasValue)
        {
            _logger.Warning("Managing pending tasks stopped. No pending tasks.");
            return;
        }
        if (!receivers.HasValue)
        {
            _logger.Warning("Managing pending tasks stopped. No receivers.");
            return;
        }
        int count = 0;
        foreach (var receiver in receivers.Value)
        {
            foreach (var task in tasks.Value)
            {
                await _client.SendMessage(receiver.Id, task.Text);
                count++;
            }
        }
        if (count == 0)
        {
            _logger.Warning("Managing pending tasks stopped. No pending tasks.");
            return;
        }
        await _removePendingTasks.Handle(new(appTime));
        _logger.Information("Managing pending tasks. Managed: {Count}", count);
    }
}
