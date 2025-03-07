namespace ReportTaskPlanner.Utilities.Converters;

public sealed record ConvertProcess
{
    public TOut Convert<TOut>(IConverter<TOut> converter)
    {
        return converter.Convert();
    }
}
