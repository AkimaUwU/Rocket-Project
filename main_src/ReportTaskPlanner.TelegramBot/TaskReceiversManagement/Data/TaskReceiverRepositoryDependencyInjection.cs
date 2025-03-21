using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;

[Injection]
public static class TaskReceiverRepositoryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<TaskReceiverDbContext>();
        services.AddScoped<TaskReceiverRepository>();
        TaskReceiverRepositoryConstants.RegisterBsonMapping();
    }
}
