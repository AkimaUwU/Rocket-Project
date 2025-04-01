using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;

[Injection]
public static class ApplicationTimeRepositoryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<IApplicationTimeRepository>(_ =>
        {
            ApplicationTimeRepository repository = new(new ApplicationTimeDbContext());
            return repository;
        });
    }
}
