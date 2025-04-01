using ReportTaskPlanner.TelegramBot.Shared.Utils;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognitions;
using ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.Recognizers;

namespace ReportTaskPlanner.TelegramBot.TimeRecognitionModule.Models.RecognitionStrategies;

public sealed class RawStringRecognitionStrategy : IRecognitionStrategy
{
    public async Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer) =>
        await recognizer.TryRecognize(
            input.ToLowerInvariant().CleanString().CleanStringFromPrepositionsAndConjunctions()
        );
}
