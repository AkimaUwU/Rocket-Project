using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using Serilog;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade.Decorators;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.CalculationWithRecognitionTests;

public sealed class BasicMonthRecognitionTests : BaseRecognitionTest
{
    private readonly ITimeRecognitionFacade _facade;
    private readonly IServiceScopeFactory _factory;

    public BasicMonthRecognitionTests()
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
    public async Task Invoke_Day_Month_Recognition_January()
    {
        string input = "16 Января в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(1, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_February()
    {
        string input = "16 Февраля в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(2, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_March()
    {
        string input = "16 Марта в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(3, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_April()
    {
        string input = "16 Апреля в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(4, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_May()
    {
        string input = "16 Мая в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(5, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_June()
    {
        string input = "16 Июня в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(6, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_July()
    {
        string input = "16 Июля в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(7, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_August()
    {
        string input = "16 Августа в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(8, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_September()
    {
        string input = "16 Сентября в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(9, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_October()
    {
        string input = "16 Октября в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(10, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_November()
    {
        string input = "16 Ноября в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(11, month);
    }

    [Fact]
    public async Task Invoke_Day_Month_Recognition_December()
    {
        string input = "16 Декабря в 23:35 сделать коммит на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        int month = calculated.DateTime.Month;
        Assert.Equal(12, month);
    }
}
