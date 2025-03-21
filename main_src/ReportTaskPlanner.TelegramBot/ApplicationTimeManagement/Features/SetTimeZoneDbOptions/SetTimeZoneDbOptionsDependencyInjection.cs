using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetTimeZoneDbOptions.Decorators;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
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
        services.AddScoped<ICommandHandler<SetTimeZoneDbOptionsCommand, TimeZoneDbOptions>>(p =>
        {
            ILogger logger = p.GetRequiredService<ILogger>();
            TimeZoneDbRepository repository = p.GetRequiredService<TimeZoneDbRepository>();
            SetTimeZoneDbOptionsCommandHandler h1 = new(repository);
            SetTimeZoneDbLogging h2 = new(h1, logger);
            return h2;
        });
    }
}
