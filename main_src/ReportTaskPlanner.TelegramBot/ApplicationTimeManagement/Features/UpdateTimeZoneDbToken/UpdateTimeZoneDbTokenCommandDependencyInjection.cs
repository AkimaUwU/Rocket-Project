using ReportTaskPlanner.TelegramBot.Shared.CqrsPattern;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features.UpdateTimeZoneDbToken;

[Injection]
public static class UpdateTimeZoneDbTokenCommandDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<
            ICommandHandler<UpdateTimeZoneDbTokenCommand, bool>,
            UpdateTimeZoneDbTokenCommandHandler
        >();
    }
}
