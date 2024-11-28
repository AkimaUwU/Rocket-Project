using RocketPlaner.Core.models.RocketTasks.Errors;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks.ValueObjects;

public record RocketTaskMessage
{
    public string Message { get; init; }

    private RocketTaskMessage(string message) => Message = message;

    public static Result<RocketTaskMessage> Create(string? message) =>
        string.IsNullOrWhiteSpace(message)
            ? RocketTaskErrors.CannotHaveEmptyMessage
            : new RocketTaskMessage(message);
}
