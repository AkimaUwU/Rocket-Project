using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.TimeZoneDbData;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones.Decorators;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.ListTimeZones;

[Injection]
public static class ListTimeZonesDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<IQueryHandler<ListTimeZonesQuery, ApplicationTime[]>>(p =>
        {
            ITimeZoneDbRepository repository = p.GetRequiredService<ITimeZoneDbRepository>();
            ILogger logger = p.GetRequiredService<ILogger>();
            ListTimeZonesSharedContext context = new();
            ListTimeZonesQueryHandler h1 = new(context);
            ListTimeZonesRusificator h2 = new(h1, context);
            ListTimeZonesResponseDeserializer h3 = new(context, h2);
            ListTimeZonesTimeZoneDbRequester h4 = new(h3, context);
            ListTimeZonesOptionsRequester h5 = new(h4, context, repository);
            ListTimeZonesLogging h6 = new(h5, context, logger);
            return h6;
        });
    }
}
