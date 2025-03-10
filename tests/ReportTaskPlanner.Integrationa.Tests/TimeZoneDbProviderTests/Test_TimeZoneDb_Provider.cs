using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.TimeZoneDbProvider.Configuration;
using ReportTaskPlanner.TimeZoneDbProvider.TimeZoneDbProviderIntegration;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Integrationa.Tests.TimeZoneDbProviderTests;

public sealed class Test_TimeZoneDb_Provider : BaseTest
{
    private static string filePath = Path.Combine("Configuration", "TIME_ZONE_DB_CONFIG.json");

    [Fact]
    public async Task List_Time_Zones()
    {
        TimeZoneDbRequester requester = GetService<TimeZoneDbRequester>();
        TimeZoneDbOptionsFileRepository repository = GetService<TimeZoneDbOptionsFileRepository>();
        TimeZoneDbOptionsJsonReader reader = GetService<TimeZoneDbOptionsJsonReader>();

        Result<TimeZoneDbOptions> optionsResult = repository.Get(filePath);
        Assert.True(optionsResult.IsSuccess);
        TimeZoneDbOptions options = optionsResult.Value;

        Result<TimeZoneDbResponseJson> json = await requester.RequestJson(options);
        Assert.True(json.IsSuccess);

        Result<IEnumerable<PlannerTime>> result = requester.ReadJson(json, reader.Read).Invoke();
        Assert.True(result.IsSuccess);

        foreach (var plannerTime in result.Value)
        {
            _logger.Information("Time Display name: {Name}", plannerTime.DisplayName);
        }
    }
}
