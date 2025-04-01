using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.DeleteTimeZoneDbToken;

[Injection]
public static class DeleteTimeZoneDbTokenCommandDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<
            ICommandHandler<DeleteTimeZoneDbTokenCommand, bool>,
            DeleteTimeZoneDbTokenCommandHandler
        >();
    }
}
