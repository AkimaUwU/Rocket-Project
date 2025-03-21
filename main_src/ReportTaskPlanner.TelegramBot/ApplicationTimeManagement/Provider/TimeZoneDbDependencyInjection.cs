using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;

[Injection]
public static class TimeZoneDbDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<TimeZoneDbContext>();
        services.AddScoped<TimeZoneDbRepository>();
        TimeZoneDbRepositoryConstants.RegisterBsonMapping();
    }
}
