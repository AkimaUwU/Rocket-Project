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
        services.AddScoped<ICommandHandler<RegisterReceiverCommand, TaskReceiver>>(p =>
        {
            Serilog.ILogger logger = p.GetRequiredService<Serilog.ILogger>();
            TaskReceiverRepository repository = p.GetRequiredService<TaskReceiverRepository>();
            RegisterReceiverCommandHandler h1 = new(repository);
            RegisterReceiverValidating h2 = new(repository, h1);
            RegisterReceiverLogging h3 = new(logger, h2);
            return h3;
        });
    }
}
