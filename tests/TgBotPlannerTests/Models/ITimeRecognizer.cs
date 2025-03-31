using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models;

public interface ITimeRecognizer
{
    Task<TimeRecognition> TryRecognize(string input);
}
