using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.Facade;

public interface ITimeRecognitionFacade
{
    Task<TimeRecognitionTicket> CreateRecognitionTicket(string message);

    Task<RecognitionMetadataCollection> CollectMetadata(TimeRecognitionTicket ticket);

    ApplicationTime GetApplicationTimeWithOffset(
        RecognitionMetadataCollection collection,
        ApplicationTime current
    );

    Task<Result<ReportTaskSchedule>> CreateTaskSchedule(string message, ApplicationTime time);
}
