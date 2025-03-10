using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.TimeZoneDbProvider.TimeZoneDbProviderIntegration;

public sealed record TimeZoneDbResponseJson
{
    public string Json { get; }
    public TimeZoneDbResponseJson(string json) => Json = json;
}

public delegate Result<IEnumerable<PlannerTime>> ReadTimeZoneDbResponseJson(TimeZoneDbResponseJson json);