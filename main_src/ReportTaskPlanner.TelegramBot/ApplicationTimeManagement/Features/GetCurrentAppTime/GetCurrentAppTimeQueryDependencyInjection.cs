using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.GetCurrentAppTime;

[Injection]
public static class GetCurrentAppTimeQueryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetCurrentAppTimeQuery, Option<ApplicationTime>>>(p =>
        {
            ApplicationTimeRepository repository =
                p.GetRequiredService<ApplicationTimeRepository>();
            return new GetCurrentAppTimeQueryHandler(repository);
        });
    }
}
