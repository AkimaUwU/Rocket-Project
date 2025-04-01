using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.RemovePendingTasks;

[Injection]
public static class RemovePendingTasksCommandDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<RemovePendingTasksCommand, int>>(p =>
        {
            IReportTaskRepository repository = p.GetRequiredService<IReportTaskRepository>();
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            RemovePendingTasksCommandHandler h1 = new(repository);
            RemovePendingTasksExceptionDecorator h2 = new(h1);
            RemovePendingTasksLoggingDecorator h3 = new(h2, logger);
            return h3;
        });
    }
}
