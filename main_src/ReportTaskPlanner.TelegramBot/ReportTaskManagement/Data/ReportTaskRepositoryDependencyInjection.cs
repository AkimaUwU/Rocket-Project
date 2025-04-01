using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data.Decorators;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Data;

[Injection]
public static class ReportTaskRepositoryDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<IReportTaskRepository>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            ReportTaskRepository repository = new(new ReportTaskDbContext());
            ReportTaskRepositoryLogging logging = new(repository, logger);
            return logging;
        });
    }
}
