using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;
using Telegram.Bot;

namespace ReportTaskPlanner.TelegramBot.TasksNotificationManagement;

public sealed class TasksNotificaitonManager(
    ReportTaskManagementApi tasksApi,
    BotPlannerTimeManagementApi timeApi,
    TaskReceiversManagementApi receiversApi,
    Serilog.ILogger logger
)
{
    private readonly ReportTaskManagementApi _tasksApi = tasksApi;
    private readonly BotPlannerTimeManagementApi _timeApi = timeApi;
    private readonly TaskReceiversManagementApi _receiversApi = receiversApi;
    private readonly Serilog.ILogger _logger = logger;

    public async Task ManagePendingTasks(TelegramBotClient client)
    {
        _logger.Information("Managing pending tasks...");
        Option<ApplicationTime> appTime = await _timeApi.GetUpdatedApplicationTime();
        if (!appTime.HasValue)
        {
            _logger.Warning("Managing pending tasks stopped. Application time is not configured.");
            return;
        }

        ApplicationTime currentTime = appTime.Value;
        Option<IEnumerable<ReportTask>> tasks = await _tasksApi.GetPendingTasks(currentTime);
        if (!tasks.HasValue)
        {
            _logger.Warning("Managing pending tasks stopped. No pending tasks.");
            return;
        }

        Option<IEnumerable<TaskReceiver>> receivers = await _receiversApi.GetReceivers();
        if (!receivers.HasValue)
        {
            _logger.Warning("Managing pending tasks stopped. No receivers.");
            return;
        }

        ReportTask[] tasksSequence = tasks.Value.ToArray();
        TaskReceiver[] receiversSequence = receivers.Value.ToArray();
        int managed = await SendTasks(tasksSequence, receiversSequence, client);
        _logger.Information("Managing pending tasks. Managed: {Count}", managed);

        tasksSequence.AdjustPeriodicTasks(currentTime);
        ReportTask[] nonPeriodic = tasksSequence.FilterBy(t => !t.Schedule.IsPeriodic).ToArray();
        ReportTask[] periodic = tasksSequence.FilterBy(t => t.Schedule.IsPeriodic).ToArray();
        _logger.Information("Request for removing tasks pending...");
        await _tasksApi.RemoveMany(nonPeriodic);
        _logger.Information("Request for removing tasks finished.");
        _logger.Information("Request for updating tasks pending...");
        await _tasksApi.UpdateMany(periodic);
        _logger.Information("Request for updating tasks finished...");
    }

    private static async Task<int> SendTasks(
        ReportTask[] tasks,
        IEnumerable<TaskReceiver> receivers,
        TelegramBotClient client
    )
    {
        int count = 0;
        foreach (TaskReceiver receiver in receivers)
        {
            foreach (ReportTask task in tasks)
            {
                client.SendMessage(receiver.Id, task.Text);
                count++;
            }
        }

        return count;
    }
}
