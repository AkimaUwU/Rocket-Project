using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteApplicationTime;

[Injection]
public static class DeleteApplicationTimeCommandDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<
            ICommandHandler<DeleteApplicationTimeCommand, bool>,
            DeleteApplicationTimeCommandHandler
        >();
    }
}
