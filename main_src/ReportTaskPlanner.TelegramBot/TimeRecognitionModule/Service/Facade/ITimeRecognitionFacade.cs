using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.ReportTaskManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Service.Facade;

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
