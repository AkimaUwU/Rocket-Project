using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using Serilog;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade.Decorators;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.CalculationWithPeriodicTimeTests;

public sealed class BasicPeriodicHourRecognitionTests : BaseRecognitionTest
{
    private readonly ITimeRecognitionFacade _facade;
    private readonly IServiceScopeFactory _factory;

    public BasicPeriodicHourRecognitionTests()
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
    public async Task Create_Every_1_Hour_Periodic_Recognition()
    {
        string input = "Каждый час заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(1, totalHours);
    }

    [Fact]
    public async Task Create_Every_2_Hour_Periodic_Recognition()
    {
        string input = "Каждые 2 часа заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(2, totalHours);
    }

    [Fact]
    public async Task Create_Every_3_Hour_Periodic_Recognition()
    {
        string input = "Каждые 3 часа заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(3, totalHours);
    }

    [Fact]
    public async Task Create_Every_4_Hour_Periodic_Recognition()
    {
        string input = "Каждые 4 часа заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(4, totalHours);
    }

    [Fact]
    public async Task Create_Every_5_Hour_Periodic_Recognition()
    {
        string input = "Каждые 5 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(5, totalHours);
    }

    [Fact]
    public async Task Create_Every_6_Hour_Periodic_Recognition()
    {
        string input = "Каждые 6 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(6, totalHours);
    }

    [Fact]
    public async Task Create_Every_7_Hour_Periodic_Recognition()
    {
        string input = "Каждые 7 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(7, totalHours);
    }

    [Fact]
    public async Task Create_Every_8_Hour_Periodic_Recognition()
    {
        string input = "Каждые 8 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(8, totalHours);
    }

    [Fact]
    public async Task Create_Every_9_Hour_Periodic_Recognition()
    {
        string input = "Каждые 9 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(9, totalHours);
    }

    [Fact]
    public async Task Create_Every_10_Hour_Periodic_Recognition()
    {
        string input = "Каждые 10 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(10, totalHours);
    }

    [Fact]
    public async Task Create_Every_11_Hour_Periodic_Recognition()
    {
        string input = "Каждые 11 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(11, totalHours);
    }

    [Fact]
    public async Task Create_Every_12_Hour_Periodic_Recognition()
    {
        string input = "Каждые 12 часов заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalHours = (int)span.TotalHours;
        Assert.Equal(12, totalHours);
    }
}
