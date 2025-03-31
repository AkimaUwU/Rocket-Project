using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models;

public interface IRecognitionStrategy
{
    Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer);
}
