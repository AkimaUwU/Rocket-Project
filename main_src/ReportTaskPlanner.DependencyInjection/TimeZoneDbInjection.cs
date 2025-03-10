using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TimeZoneDbProvider;

namespace ReportTaskPlanner.DependencyInjection;

public static class TimeZoneDbInjection
{
    public static void InjectTimeZoneDb(this IServiceCollection services)
    {
        TimeZoneDbOptionsFileRepository repository = new(
            TimeZoneDbOptionsActions.GetTimeZoneDbOptions, 
            TimeZoneDbOptionsActions.SaveTimeZoneDbOptions,
            TimeZoneDbOptionsActions.UpdateTimeZoneDbOptionsFile
        );
        services.AddSingleton(repository);
    }
}