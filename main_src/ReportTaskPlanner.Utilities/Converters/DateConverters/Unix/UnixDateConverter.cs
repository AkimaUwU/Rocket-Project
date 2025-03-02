namespace ReportTaskPlanner.Utilities.Converters.DateConverters.Unix;

public abstract record UnixDateConverter : IConverter<long>
{
    public abstract long Convert();
}

public sealed record FromDateTimeConverter(DateTime date) : UnixDateConverter
{
    private readonly DateTime _minimalValue = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override long Convert()
    {
        return (long)(date - _minimalValue).TotalSeconds;
    }
}
