using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.TasksNotificationManagement;

[Injection]
public static class TasksNotificaitonManagerDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddSingleton<TasksNotificaitonManager>();
    }
}
