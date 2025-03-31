using ReportTaskPlanner.TelegramBot.Shared.Utils;
using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models.RecognitionStrategies;

public sealed class RawStringRecognitionStrategy : IRecognitionStrategy
{
    public async Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer) =>
        await recognizer.TryRecognize(
            input.ToLowerInvariant().CleanString().CleanStringFromPrepositionsAndConjunctions()
        );
}
