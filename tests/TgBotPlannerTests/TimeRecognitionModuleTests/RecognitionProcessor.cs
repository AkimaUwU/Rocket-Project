using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests;

public sealed class RecognitionProcessor
{
    private readonly List<ITimeRecognizer> _recognizers = [];

    public IRecognitionStrategy? Strategy { get; set; } = null;

    public void AddRecognizer(ITimeRecognizer recognizer) => _recognizers.Add(recognizer);

    public void AddRecognizer(params ITimeRecognizer[] recognizers) =>
        _recognizers.AddRange(recognizers);

    public void ResetRecognizers() => _recognizers.Clear();

    public async Task<TimeRecognition> PerformRecognition(string input)
    {
        if (Strategy == null)
            return new UnrecognizedTime();
        foreach (ITimeRecognizer recognizer in _recognizers)
        {
            TimeRecognition recognition = await Strategy.Recognize(input, recognizer);
            if (ShouldStop(recognition))
                return recognition;
        }

        return new UnrecognizedTime();
    }

    private static bool ShouldStop(TimeRecognition recognition) =>
        recognition switch
        {
            UnrecognizedTime => false,
            _ => true,
        };
}
