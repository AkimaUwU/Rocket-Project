using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime.Decorators;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.SetApplicationTime;

[Injection]
public static class SetApplicationTimeDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<SetApplicationTimeCommand, ApplicationTime>>(p =>
        {
            ILogger logger = p.GetRequiredService<ILogger>();
            IApplicationTimeRepository repository =
                p.GetRequiredService<IApplicationTimeRepository>();
            SetApplicationTimeCommandHandler h1 = new(repository);
            SetApplicationTimeLogging h2 = new(logger, h1);
            return h2;
        });
    }
}
