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
        services.AddScoped<
            IQueryHandler<GetUpdatedApplicationTimeQuery, Option<ApplicationTime>>,
            GetUpdatedApplicationTimeQueryHandler
        >();
    }
}
