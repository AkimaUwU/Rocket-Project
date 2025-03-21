using ReportTaskPlanner.TelegramBot.TasksNotificationManagement;

namespace ReportTaskPlanner.TelegramBot;

public class Worker(IServiceScopeFactory factory) : BackgroundService
{
    private readonly IServiceScopeFactory _factory = factory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //try
            //{
            using IServiceScope scope = _factory.CreateScope();
            IServiceProvider provider = scope.ServiceProvider;
            TasksNotificaitonManager manager =
                provider.GetRequiredService<TasksNotificaitonManager>();
            await manager.ManagePendingTasks();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            // ignored.
            //}
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
