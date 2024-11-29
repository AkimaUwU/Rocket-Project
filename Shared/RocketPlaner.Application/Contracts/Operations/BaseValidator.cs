using RocketPlaner.Core.Tools;

namespace RocketPlaner.Application.Contracts.Operations;

public abstract class BaseValidator
{
    protected readonly List<Error> errors = [];

    protected void AddErrorFromResult<T>(Result<T> result)
    {
        if (result.IsError)
            errors.Add(result.Error);
    }

    public bool HasErrors => errors.Count == 0;

    public Error LastError => errors.Count == 0 ? Error.None : errors.Last();
}
