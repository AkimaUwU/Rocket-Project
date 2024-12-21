using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using RocketPlanner.TimeClassifier.Data;

namespace RocketPlanner.TimeClassifier.Classificator;

public sealed class TimeClassifier
{
    private readonly MLContext _context = new MLContext();
    private readonly ITransformer _trainedModel;
    private const string Path = "timeClassifier.zip";

    public TimeClassifier(InMemoryTrainingData data)
    {
        _trainedModel = _context.Model.Load(Path, out var modelInputSchema);
    }

    public PredictionEngine<InputTime, TimePrediction> CreatePredictionEngine()
    {
        return _context.Model.CreatePredictionEngine<InputTime, TimePrediction>(_trainedModel);
    }
}

public static class TimeClassifierExtensions
{
    public static async Task<TimePrediction> PredictAsync(
        this PredictionEngine<InputTime, TimePrediction> predictionEngine,
        string input
    )
    {
        InputTime inputTime = new InputTime() { Text = input };
        TimePrediction prediction = predictionEngine.Predict(inputTime);
        return await Task.FromResult(prediction);
    }
}
