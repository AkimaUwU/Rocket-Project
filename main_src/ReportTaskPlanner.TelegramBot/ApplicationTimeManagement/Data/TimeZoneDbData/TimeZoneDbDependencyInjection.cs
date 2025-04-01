using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;

[Injection]
public static class TimeZoneDbDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ITimeZoneDbRepository>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            TimeZoneDbRepository repository = new(new TimeZoneDbContext());
            TimeZoneDbLoggingDecorator logging = new(repository, logger);
            return logging;
        });
    }
}
