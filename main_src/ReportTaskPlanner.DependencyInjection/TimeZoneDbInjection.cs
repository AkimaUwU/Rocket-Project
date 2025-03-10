using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TimeZoneDbProvider.Configuration;
using ReportTaskPlanner.TimeZoneDbProvider.TimeZoneDbProviderIntegration;

namespace ReportTaskPlanner.DependencyInjection;

public static class TimeZoneDbInjection
{
    public static void InjectTimeZoneDb(this IServiceCollection services)
    {
        TimeZoneDbOptionsFileRepository repository = new(
            TimeZoneDbOptions.Actions.GetTimeZoneDbOptions,
            TimeZoneDbOptions.Actions.SaveTimeZoneDbOptions,
            TimeZoneDbOptions.Actions.UpdateTimeZoneDbOptionsFile
        );

        TimeZoneDbOptionsJsonReader reader = new(TimeZoneDbOptionsJsonReader.Actions.Read);

        TimeZoneDbRequester requester = new TimeZoneDbRequester(
            TimeZoneDbRequester.Actions.ListPlannerTimesFromTimeZoneDbProvider,
            TimeZoneDbRequester.Actions.GetTimeZoneDbResponseJson,
            TimeZoneDbRequester.Actions.SaveGlobalPlannerTime,
            TimeZoneDbRequester.Actions.UpdatePlannerTimeFromRequest,
            TimeZoneDbRequester.Actions.ReadFromJsonFile
        );

        services.AddTransient<TimeZoneDbOptionsFileRepository>(_ => repository);
        services.AddTransient<TimeZoneDbOptionsJsonReader>(_ => reader);
        services.AddTransient<TimeZoneDbRequester>(_ => requester);
    }
}
