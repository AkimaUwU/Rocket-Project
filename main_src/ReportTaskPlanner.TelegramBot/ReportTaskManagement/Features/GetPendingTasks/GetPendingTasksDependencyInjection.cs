using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.GetPendingTasks;

[Injection]
public static class GetPendingTasksDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<
            IQueryHandler<GetPendingTasksQuery, Option<IEnumerable<ReportTask>>>,
            GetPendingTasksQueryHandler
        >();
    }
}
