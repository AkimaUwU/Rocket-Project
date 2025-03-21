using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Provider;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetTimeZoneDbOptions;

[Injection]
public static class GetTimeZoneDbOptionsDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetTimeZoneDbOptionsQuery, Option<TimeZoneDbOptions>>>(p =>
        {
            TimeZoneDbRepository repository = p.GetRequiredService<TimeZoneDbRepository>();
            return new GetTimeZoneDbOptionsQueryHandler(repository);
        });
    }
}
