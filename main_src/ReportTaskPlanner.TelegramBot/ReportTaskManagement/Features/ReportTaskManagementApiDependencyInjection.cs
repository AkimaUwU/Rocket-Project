using ReportTaskPlanner.TelegramBot.Shared.Extensions;

namespace ReportTaskPlanner.TelegramBot.ReportTaskManagement.Features;

[Injection]
public static class ReportTaskManagementApiDependencyInjection
{
    [InjectionMethod]
    public static void Inject(this IServiceCollection services)
    {
        services.AddTransient<ReportTaskManagementApi>();
    }
}
