using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests;

public interface IRecognitionStrategy
{
    Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer);
}
