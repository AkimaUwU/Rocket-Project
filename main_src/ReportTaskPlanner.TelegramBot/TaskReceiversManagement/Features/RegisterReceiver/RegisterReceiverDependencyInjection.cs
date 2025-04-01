using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Data;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver.Decorators;
using ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Features.RegisterReceiver;

[Injection]
public static class RegisterReceiverDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<RegisterReceiverCommand, TaskReceiver>>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            ITaskReceiverRepository repository = p.GetRequiredService<ITaskReceiverRepository>();
            RegisterReceiverCommandHandler h1 = new(repository);
            RegisterReceiverLogging h2 = new(logger, h1);
            return h2;
        });
    }
}
