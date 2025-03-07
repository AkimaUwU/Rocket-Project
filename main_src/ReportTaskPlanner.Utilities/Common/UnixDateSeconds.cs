using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.Utilities.Common;

public sealed record UnixDateSeconds
{
    public long Seconds { get; private init; }

    public static UnixDateSeconds Zero => new UnixDateSeconds(0);

    public UnixDateSeconds(long seconds)
    {
        Seconds = seconds;
    }
}

public static class UnixDateSecondsExtensions
{
    public static UnixDateSeconds GetDifferenceFrom(
        this UnixDateSeconds left,
        UnixDateSeconds right
    )
    {
        return new UnixDateSeconds(left.Seconds - right.Seconds);
    }
}
