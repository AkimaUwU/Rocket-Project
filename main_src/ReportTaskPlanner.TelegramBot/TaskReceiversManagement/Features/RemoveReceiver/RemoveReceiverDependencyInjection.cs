using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver.Decorators;
using Serilog;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver;

[Injection]
public static class RemoveReceiverDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<RemoveReceiverCommand, long>>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            TaskReceiverRepository repository = p.GetRequiredService<TaskReceiverRepository>();
            RemoveReceiverCommandHandler h1 = new(repository);
            RemoveReceiverLogging h2 = new(logger, h1);
            return h1;
        });
    }
}
