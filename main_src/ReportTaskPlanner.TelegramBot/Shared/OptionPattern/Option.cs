namespace ReportTaskPlanner.TelegramBot.Shared.OptionPattern;

public sealed class Option<T>
{
    private readonly T? _value;
    public T Value => _value ?? throw new NullReferenceException();

    public bool HasValue { get; }

    private Option()
    {
        _value = default!;
        HasValue = false;
    }

    private Option(T value)
    {
        _value = value;
        HasValue = true;
    }

    public static Option<T> Some(T value) => new Option<T>(value);

    public static Option<T> None() => new Option<T>();

    public static implicit operator T(Option<T> option) =>
        !option.HasValue ? throw new NullReferenceException() : option.Value;
}
