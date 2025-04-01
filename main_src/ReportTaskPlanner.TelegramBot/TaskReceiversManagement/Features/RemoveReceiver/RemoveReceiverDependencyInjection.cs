using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver.Decorators;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RemoveReceiver;

[Injection]
public static class RemoveReceiverDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<RemoveReceiverCommand, bool>>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            ITaskReceiverRepository repository = p.GetRequiredService<ITaskReceiverRepository>();
            RemoveReceiverCommandHandler h1 = new(repository);
            RemoveReceiverLogging h2 = new(logger, h1);
            return h2;
        });
    }
}
