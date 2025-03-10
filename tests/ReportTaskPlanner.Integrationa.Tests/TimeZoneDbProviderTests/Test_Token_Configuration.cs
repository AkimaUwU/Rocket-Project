using ReportTaskPlanner.TimeZoneDbProvider;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Integrationa.Tests.TimeZoneDbProviderTests;

public sealed class Test_Token_Configuration : BaseTest
{
    private static string filePath = Path.Combine("Configuration", "TIME_ZONE_DB_CONFIG.json");
    private const string tokenValue = "Test Token";
    private const string updatedTokenValue = "Test Updated";
    
    [Fact]
    public void Test_Save_Configuration()
    {
        TimeZoneDbOptionsFileRepository repository = GetService<TimeZoneDbOptionsFileRepository>();
        Result saving = repository.Save(filePath, tokenValue);
        Assert.True(saving.IsSuccess);
    }

    [Fact]
    public void Read_Configuration()
    {
        TimeZoneDbOptionsFileRepository repository = GetService<TimeZoneDbOptionsFileRepository>();
        Result<TimeZoneDbOptions> options = repository.Get(filePath);
        Assert.True(options.IsSuccess);
        Assert.Equal(tokenValue, options.Value.Token);
    }

    [Fact]
    public void Update_Configuration()
    {
        TimeZoneDbOptionsFileRepository repository = GetService<TimeZoneDbOptionsFileRepository>();
        TimeZoneDbOptions newOptions = new TimeZoneDbOptions(updatedTokenValue);
        Result updateResult = repository.Update(filePath, newOptions);
        Assert.True(updateResult.IsSuccess);
        
        Result<TimeZoneDbOptions> options = repository.Get(filePath);
        Assert.True(options.IsSuccess);
        Assert.Equal(newOptions, options.Value);
    }
}