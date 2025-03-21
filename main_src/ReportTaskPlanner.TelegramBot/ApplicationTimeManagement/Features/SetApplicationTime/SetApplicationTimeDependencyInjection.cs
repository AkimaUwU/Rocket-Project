using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;
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
        services.AddScoped<ICommandHandler<SetApplicationTimeCommand, ApplicationTime>>(p =>
        {
            ILogger logger = p.GetRequiredService<ILogger>();
            ApplicationTimeRepository repository =
                p.GetRequiredService<ApplicationTimeRepository>();
            SetApplicationTimeCommandHandler h1 = new SetApplicationTimeCommandHandler(repository);
            SetApplicationTimeLogging h2 = new SetApplicationTimeLogging(logger, h1);
            return h2;
        });
    }
}
