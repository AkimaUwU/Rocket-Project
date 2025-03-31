using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models.Facade;

public interface ITimeRecognitionFacade
{
    Task<TimeRecognitionTicket> CreateRecognitionTicket(string message);

    Task<RecognitionMetadataCollection> CollectMetadata(TimeRecognitionTicket ticket);

    ApplicationTime GetApplicationTimeWithOffset(
        RecognitionMetadataCollection collection,
        ApplicationTime current
    );

    Task<Result<ApplicationTime>> CreateRecognitionTime(string message, ApplicationTime time);
}
