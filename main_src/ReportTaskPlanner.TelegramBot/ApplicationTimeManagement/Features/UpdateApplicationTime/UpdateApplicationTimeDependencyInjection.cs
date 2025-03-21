using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime.Decorators;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;

[Injection]
public static class UpdateApplicationTimeDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>>(p =>
        {
            ApplicationTimeRepository appTimeRepository =
                p.GetRequiredService<ApplicationTimeRepository>();
            ILogger logger = p.GetRequiredService<ILogger>();

            UpdateApplicationTimeSharedContext context = new();
            UpdateApplicationTimeCommandHandler h1 = new(appTimeRepository, context);
            UpdateApplicationTimeDeserializing h2 = new(h1, context);
            UpdateApplicationTimeTimeZoneDbRequesting h3 = new(h2, context);
            UpdateApplicationTimeLogging h4 = new(h3, logger);

            return h4;
        });
    }
}
