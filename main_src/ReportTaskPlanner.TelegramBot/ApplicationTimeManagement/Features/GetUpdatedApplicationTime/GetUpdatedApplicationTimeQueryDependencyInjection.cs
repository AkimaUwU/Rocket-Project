using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime.Decorators;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateApplicationTime;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetUpdatedApplicationTime;

[Injection]
public static class GetUpdatedApplicationTimeQueryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<
            IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>
        >(p =>
        {
            IApplicationTimeRepository appTimeRepository =
                p.GetRequiredService<IApplicationTimeRepository>();
            ITimeZoneDbRepository timeZoneDbRepository =
                p.GetRequiredService<ITimeZoneDbRepository>();
            ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime> updater =
                p.GetRequiredService<
                    ICommandHandler<UpdateApplicationTimeCommand, ApplicationTime>
                >();
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            GetUpdatedApplicationTimeQueryHandler h1 = new(
                appTimeRepository,
                timeZoneDbRepository,
                updater
            );
            GetUpdatedApplicationTimeExceptionDecorator h2 = new(h1, logger);
            GetUpdatedApplicationTimeLoggingDecorator h3 = new(h2, logger);
            return h3;
        });
    }
}
