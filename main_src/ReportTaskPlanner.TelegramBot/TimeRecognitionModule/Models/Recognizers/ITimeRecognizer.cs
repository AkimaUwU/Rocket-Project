using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognizers;

public interface ITimeRecognizer
{
    Task<TimeRecognition> TryRecognize(string input);
}
