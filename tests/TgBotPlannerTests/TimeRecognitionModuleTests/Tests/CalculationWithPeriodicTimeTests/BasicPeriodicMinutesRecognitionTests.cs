using Microsoft.Extensions.DependencyInjection;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.Extensions;
using Serilog;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade.Decorators;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.CalculationWithPeriodicTimeTests;

public sealed class BasicPeriodicMinutesRecognitionTests : BaseRecognitionTest
{
    private readonly ITimeRecognitionFacade _facade;
    private readonly IServiceScopeFactory _factory;

    public BasicPeriodicMinutesRecognitionTests()
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
    public async Task Create_Every_1_Minute_Periodic_Recognition()
    {
        string input = "Каждую минуту заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalMinutes = (int)span.TotalMinutes;
        Assert.Equal(1, totalMinutes);
    }

    [Fact]
    public async Task Create_Every_1_Minute_Periodic_Recognition_Sample_2()
    {
        string input = "Каждую 1 минуту заливать на гитхаб.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        ApplicationTime currentTime = await GetCurrentTime(_factory);
        ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(collection, currentTime);
        TimeSpan span = calculated.DateTime - currentTime.DateTime;
        int totalMinutes = (int)span.TotalMinutes;
        Assert.Equal(1, totalMinutes);
    }

    [Fact]
    public async Task Create_Every_X_Minute_Periodic_Recognition()
    {
        for (int i = 2; i <= 59; i++)
        {
            string input = $"Каждые {i} минуты заливать на гитхаб.";
            TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
            RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
            ApplicationTime currentTime = await GetCurrentTime(_factory);
            ApplicationTime calculated = _facade.GetApplicationTimeWithOffset(
                collection,
                currentTime
            );
            TimeSpan span = calculated.DateTime - currentTime.DateTime;
            int totalMinutes = (int)span.TotalMinutes;
            Assert.Equal(i, totalMinutes);
        }
    }
}
