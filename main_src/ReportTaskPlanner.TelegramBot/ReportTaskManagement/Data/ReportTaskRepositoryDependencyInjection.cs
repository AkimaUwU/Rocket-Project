using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

[Injection]
public static class ReportTaskRepositoryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddSingleton<ReportTaskDbContext>();
        services.AddSingleton<ReportTaskRepository>();
        ReportTaskRepositoryConstants.RegisterMapping();
    }
}
