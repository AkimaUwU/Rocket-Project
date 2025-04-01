using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features;

[Injection]
public static class TaskReceiversManagmentApiInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<TaskReceiversManagementApi>();
    }
}
