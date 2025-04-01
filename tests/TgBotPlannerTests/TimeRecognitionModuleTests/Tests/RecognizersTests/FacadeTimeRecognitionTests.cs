using Serilog;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade;
using TgBotPlannerTests.TimeRecognitionModuleTests.Facade.Decorators;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Tests.RecognizersTests;

public sealed class FacadeTimeRecognitionTests
{
    private readonly ITimeRecognitionFacade _facade;

    public FacadeTimeRecognitionTests()
    {
        ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        TimeRecognitionFacade facade = new();
        TimeRecognitionFacadeLoggingDecorator logging = new(facade, logger);
        _facade = logging;
    }

    [Fact]
    public async Task Recognize_Non_Periodic_Time()
    {
        string input = "В понедельник в 14:35 будет собрание.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        Assert.True(RecognitionValidator.CanProcessTicket(ticket));
    }

    [Fact]
    public async Task Recognize_Periodic_Time()
    {
        string input = "Каждый понедельник в 20:55 будет происходить собрание.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        Assert.True(RecognitionValidator.CanProcessTicket(ticket));
    }

    [Fact]
    public async Task Collect_Recognition_Metadata_From_Periodic_Time()
    {
        string input = "В понедельник в 14:35 будет собрание.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        Assert.True(collection.Count > 0);
    }

    [Fact]
    public async Task Collect_Recognition_Metadata_From_Date_WithoutTime_Set()
    {
        string input = "В понедельник будет происходить собрание";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        Assert.Equal(0, collection.Count);
    }

    [Fact]
    public async Task Collect_Recognition_Metadata_From_Date_With_Month()
    {
        string input = "1 Апреля в 9:35 сделать зарядку.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        Assert.True(collection.Count > 0);
    }

    [Fact]
    public async Task Collect_Recognition_Metadata_From_NoTimeContext()
    {
        string input = "Это сообщение не имеет контекста времени.";
        TimeRecognitionTicket ticket = await _facade.CreateRecognitionTicket(input);
        RecognitionMetadataCollection collection = await _facade.CollectMetadata(ticket);
        Assert.Equal(0, collection.Count);
    }
}
