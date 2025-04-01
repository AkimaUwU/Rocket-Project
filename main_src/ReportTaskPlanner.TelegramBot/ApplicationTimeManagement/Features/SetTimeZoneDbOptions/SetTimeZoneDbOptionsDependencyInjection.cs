using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions;

[Injection]
public static class SetTimeZoneDbOptionsDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>>(p =>
        {
            ILogger logger = p.GetRequiredService<ILogger>();
            ITimeZoneDbRepository repository = p.GetRequiredService<ITimeZoneDbRepository>();
            SetTimeZoneDbOptionsCommandHandler h1 = new(repository);
            SetTimeZoneDbLogging h2 = new(h1, logger);
            return h2;
        });
    }
}
