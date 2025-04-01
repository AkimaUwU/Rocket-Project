using ReportTaskPlanner.TelegramBot.Shared.Utils;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.RecognitionStrategies;

public sealed class RawStringRecognitionStrategy : IRecognitionStrategy
{
    public async Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer) =>
        await recognizer.TryRecognize(
            input.ToLowerInvariant().CleanString().CleanStringFromPrepositionsAndConjunctions()
        );
}
