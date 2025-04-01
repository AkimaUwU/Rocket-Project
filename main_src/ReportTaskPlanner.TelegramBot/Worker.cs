using ReportTaskPlanner.TelegramBot.TasksNotificationManagement;
using Telegram.Bot;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot;

public class Worker(TasksNotificaitonManager manager, TelegramBotClient client, ILogger logger)
    : BackgroundService
{
    private readonly TasksNotificaitonManager _manager = manager;
    private readonly TelegramBotClient _client = client;
    private readonly ILogger _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.Information("{Context} start managing pending tasks...", nameof(Worker));
                await _manager.ManagePendingTasks(_client);
                _logger.Information("{Context} pending tasks managed.", nameof(Worker));
            }
            catch (Exception ex)
            {
                _logger.Fatal(
                    "{Context} exception at managing pending tasks. {Message}",
                    nameof(Worker),
                    ex.Message
                );
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
