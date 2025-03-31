using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using Serilog;
using TgBotPlannerTests.Models.Facade;
using TgBotPlannerTests.Models.Facade.Decorators;

namespace TgBotPlannerTests.Models.Tests.CalculationWithPeriodicTimeTests;

public sealed class BasicPeriodicWeekdayRecognitionTests : BaseRecognitionTest
{
    private readonly ITimeRecognitionFacade _facade;
    private readonly IServiceScopeFactory _factory;

    public BasicPeriodicWeekdayRecognitionTests()
    {
        ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        TimeRecognitionFacade facade = new();
        TimeRecognitionFacadeLoggingDecorator logging = new(facade, logger);
        _facade = logging;
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(logger);
        services.InjectAllServices();
        IServiceProvider provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    [Fact]
    public async Task Create_Every_Monday_At_Specific_Time()
    {
        string input = "Каждый понедельник в 18:22 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Monday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(18, calculated.Value.DateTime.Hour);
        Assert.Equal(22, calculated.Value.DateTime.Minute);
    }

    [Fact]
    public async Task Create_Every_Tuesday_At_Specific_Time()
    {
        string input = "Каждый вторник в 14:33 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Tuesday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(14, calculated.Value.DateTime.Hour);
        Assert.Equal(33, calculated.Value.DateTime.Minute);
    }

    [Fact]
    public async Task Create_Every_Wednesday_At_Specific_Time()
    {
        string input = "Каждую среду в 9:52 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Wednesday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(9, calculated.Value.DateTime.Hour);
        Assert.Equal(52, calculated.Value.DateTime.Minute);
    }

    [Fact]
    public async Task Create_Every_Thursday_At_Specific_Time()
    {
        string input = "Каждый четверг в 13:35 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Thursday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(13, calculated.Value.DateTime.Hour);
        Assert.Equal(35, calculated.Value.DateTime.Minute);
    }

    [Fact]
    public async Task Create_Every_Friday_At_Specific_Time()
    {
        string input = "Каждую пятницу в 21:25 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Friday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(21, calculated.Value.DateTime.Hour);
        Assert.Equal(25, calculated.Value.DateTime.Minute);
    }

    [Fact]
    public async Task Create_Every_Saturday_At_Specific_Time()
    {
        string input = "Каждую субботу в 14:53 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Saturday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(14, calculated.Value.DateTime.Hour);
        Assert.Equal(53, calculated.Value.DateTime.Minute);
    }

    [Fact]
    public async Task Create_Every_Sunday_At_Specific_Time()
    {
        string input = "Каждое воскресенье в 10:30 заканчивать работу";
        ApplicationTime time = await GetCurrentTime(_factory);
        Result<ApplicationTime> calculated = await _facade.CreateRecognitionTime(input, time);
        Assert.True(calculated.IsSuccess);
        Assert.Equal(DayOfWeek.Sunday, calculated.Value.DateTime.DayOfWeek);
        Assert.Equal(10, calculated.Value.DateTime.Hour);
        Assert.Equal(30, calculated.Value.DateTime.Minute);
    }
}
