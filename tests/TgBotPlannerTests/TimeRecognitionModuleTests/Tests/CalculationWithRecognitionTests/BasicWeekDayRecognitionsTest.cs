using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using Serilog;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade.Decorators;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.CalculationWithRecognitionTests;

public sealed class BasicWeekDayRecognitionsTest : BaseRecognitionTest
{
    private readonly ITimeRecognitionFacade _facade;
    private readonly IServiceScopeFactory _factory;

    public BasicWeekDayRecognitionsTest()
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
    public async Task Invoke_Day_Of_WeekTest_Monday()
    {
        string input = "В понедельник в 23:55 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Monday, calculated.DateTime.DayOfWeek);
    }

    [Fact]
    public async Task Invoke_Day_Of_WeekTest_Tuesday()
    {
        string input = "Во вторник в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Tuesday, calculated.DateTime.DayOfWeek);
    }

    [Fact]
    public async Task Invoke_Day_Of_WeekTest_Wednesday()
    {
        string input = "В среду в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Wednesday, calculated.DateTime.DayOfWeek);
    }

    [Fact]
    public async Task Invoke_Day_Of_WeekTest_Thursday()
    {
        string input = "В четверг в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Thursday, calculated.DateTime.DayOfWeek);
    }

    [Fact]
    public async Task Invoke_Day_Of_WeekTest_Friday()
    {
        string input = "В пятницу в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Friday, calculated.DateTime.DayOfWeek);
    }

    [Fact]
    public async Task Invoke_Day_Of_WeekTest_Saturday()
    {
        string input = "В субботу в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Saturday, calculated.DateTime.DayOfWeek);
    }

    [Fact]
    public async Task Invoke_Day_Of_WeekTest_Sunday()
    {
        string input = "В воскресенье в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        Assert.Equal(DayOfWeek.Sunday, calculated.DateTime.DayOfWeek);
    }
}
