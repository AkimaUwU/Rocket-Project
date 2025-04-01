using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Features;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.BotEndpoints.BotPlannerTimeManagement;

[Injection]
public class BotPlannerTimeManagementApiDependencyInjection
{
    [InjectionMethod]
    public static void Inject(IServiceCollection services)
    {
        services.AddTransient<BotPlannerTimeManagementApi>();
    }
}
