using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;

[Injection]
public static class ApplicationTimeRepositoryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddSingleton<ApplicationTimeDbContext>();
        services.AddSingleton<ApplicationTimeRepository>();
        ApplicationTimeRepositoryConstants.RegisterBsonMapping();
    }
}
