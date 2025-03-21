using ReportTaskPlanner.TelegramBot.Shared.OptionPattern;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.Shared.Decorators;

public abstract class SharedContext
{
    private Option<Error> _error = Option<Error>.None();

    public void SetError(Error error)
    {
        if (_error.HasValue)
            return;
        _error = Option<Error>.Some(error);
    }

    public void SetError(string error)
    {
        if (_error.HasValue)
            return;
        Error errorModel = new Error(error);
        SetError(errorModel);
    }

    public bool HasError => _error.HasValue;

    public Error Error => _error.Value;
}
