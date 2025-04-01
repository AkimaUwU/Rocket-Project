using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.UpdateTasks.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features.UpdateTasks;

[Injection]
public static class UpdateTasksCommandDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<UpdateTasksCommand, int>>(p =>
        {
            IReportTaskRepository repository = p.GetRequiredService<IReportTaskRepository>();
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            UpdateTasksCommandHandler h1 = new(repository);
            UpdateTasksExceptionDecorator h2 = new(h1);
            UpdateTasksLoggingDecorator h3 = new(h2, logger);
            return h3;
        });
    }
}
