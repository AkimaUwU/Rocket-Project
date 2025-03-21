using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks;

[Injection]
public static class RemovePendingTasksCommandDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<
            ICommandHandler<RemovePendingTasksCommand, bool>,
            RemovePendingTasksCommandHandler
        >();
    }
}
