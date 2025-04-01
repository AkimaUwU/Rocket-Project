using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests;

public interface ITimeRecognizer
{
    Task<TimeRecognition> TryRecognize(string input);
}
