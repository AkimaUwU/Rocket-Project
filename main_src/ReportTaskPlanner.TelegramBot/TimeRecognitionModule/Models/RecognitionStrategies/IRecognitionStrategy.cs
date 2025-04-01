using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognizers;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.RecognitionStrategies;

public interface IRecognitionStrategy
{
    Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer);
}
