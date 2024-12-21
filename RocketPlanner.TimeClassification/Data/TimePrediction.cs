namespace RocketPlanner.TimeClassifier.Data;

public sealed class TimePrediction
{
    public string PredictedLabel { get; set; } = string.Empty;
    public float[] Score { get; set; } = Array.Empty<float>();
}
