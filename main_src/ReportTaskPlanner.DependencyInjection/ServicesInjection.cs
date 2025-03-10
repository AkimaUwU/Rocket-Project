using Microsoft.Extensions.DependencyInjection;

namespace ReportTaskPlanner.DependencyInjection;

public static class ServicesInjection
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.InjectLogger();
        services.InjectLiteDb();
        services.InjectUseCases();
        services.InjectTimeZoneDb();
    }
}
