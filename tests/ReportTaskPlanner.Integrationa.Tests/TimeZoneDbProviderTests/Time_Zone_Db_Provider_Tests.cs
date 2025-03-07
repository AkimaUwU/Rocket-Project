using System.Globalization;
using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.TimeZoneDbProvider;

namespace ReportTaskPlanner.Integrationa.Tests.TimeZoneDbProviderTests;

public class Time_Zone_Db_Provider_Tests
{
    private const string token = "N75LSH9ABRHP";
    private static string configurationFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration");
    private static string path = Path.Combine(configurationFolderPath, "TIME_ZONE_DB_CONFIG.json");

    public Time_Zone_Db_Provider_Tests()
    {
        if (!Directory.Exists(configurationFolderPath))
            Directory.CreateDirectory(configurationFolderPath);
    }
    
    [Fact]
    public void Create_Time_Zone_Db_Options()
    {
        TimeZoneDbOptions options = TimeZoneDbOptionsManager.CreateNewTimeZoneDbOptionsFile(token, path);
        Assert.Equal(token, options.Token);
    }

    [Fact]
    public async Task List_Planner_Time_Enumerable()
    {
        TimeZoneDbOptions options = TimeZoneDbOptionsManager.GetCurrentTimeZoneDbOptions(path);
        TimeZoneDbProvider.TimeZoneDbProvider provider = new TimeZoneDbProvider.TimeZoneDbProvider(options);
        IEnumerable<PlannerTime> time = await provider.ListTimes();
        foreach (var element in time)
        {
            Console.WriteLine($"Time zone: {element.DisplayName} / Zone Name: {element.ZoneName} / Unix time: {element.TimeStamp} / Date time: {element.DateTime.ToString("HH:mm:ss")}");
        }
    }
}