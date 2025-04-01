using ReportTaskPlanner.TelegramBot.Shared.Utils;
using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests.RecognitionStrategies;

public sealed class ChunkRecognitionStrategy : IRecognitionStrategy
{
    public async Task<TimeRecognition> Recognize(string input, ITimeRecognizer recognizer)
    {
        string[] words = SplitIntoWords(
            input.ToLowerInvariant().CleanString().CleanStringFromPrepositionsAndConjunctions()
        );

        foreach (string word in words)
        {
            TimeRecognition result = await recognizer.TryRecognize(word);
            bool shouldStop = ShouldStop(result);
            if (shouldStop)
                return result;
        }

        return new UnrecognizedTime();
    }

    private static string[] SplitIntoWords(string input) =>
        input.Split(' ', StringSplitOptions.TrimEntries);

    private static bool ShouldStop(TimeRecognition recognition) =>
        recognition switch
        {
            UnrecognizedTime => false,
            _ => true,
        };
}
