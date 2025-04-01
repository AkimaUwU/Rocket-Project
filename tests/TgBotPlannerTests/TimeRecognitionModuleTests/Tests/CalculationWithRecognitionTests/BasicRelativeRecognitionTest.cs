using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using Serilog;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade.Decorators;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.CalculationWithRecognitionTests;

public sealed class BasicRelativeRecognitionTest : BaseRecognitionTest
{
    private readonly ITimeRecognitionFacade _facade;
    private readonly IServiceScopeFactory _factory;

    public BasicRelativeRecognitionTest()
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
    public async Task Invoke_Relative_Tomorrow()
    {
        string input = "завтра в 18:36 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int daysDifference = (int)Math.Ceiling(span.TotalDays);
        if (daysDifference < 1 && span.TotalDays > 0)
        {
            daysDifference = 1;
        }

        Assert.Equal(1, daysDifference);
    }

    [Fact]
    public async Task Invoke_Relative_Day_Today()
    {
        string input = "Сегодня в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int daysDifference;
        if (span is { TotalDays: < 1, TotalHours: < 1 })
        {
            daysDifference = 0;
        }
        else
        {
            daysDifference = (int)Math.Floor(span.TotalDays);
        }

        Assert.Equal(0, daysDifference);
    }

    [Fact]
    public async Task Invoke_Relative_Day_AfterTomorrow()
    {
        string input = "Послезавтра в 23:58 назначить генеральную уборку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int daysDifference = (int)Math.Ceiling(span.TotalDays);
        Assert.Equal(2, daysDifference);
    }
}
