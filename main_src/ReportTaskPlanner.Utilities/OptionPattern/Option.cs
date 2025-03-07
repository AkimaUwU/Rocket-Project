namespace ReportTaskPlanner.Utilities.OptionPattern;

public record Option<TValue>
{
    private readonly TValue _value = default!;

    public bool HasValue { get; protected init; }

    public TValue Value
    {
        get => HasValue ? _value : throw new ApplicationException("Cannot access body of none");
        private init => _value = value;
    }

    public static Option<U> Some<U>(U value) => new Option<U>() { Value = value, HasValue = true };

    public static Option<U> None<U>() => new Option<U>() { Value = default!, HasValue = false };
}
