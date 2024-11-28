using RocketPlaner.Core.models.RocketTasks.Errors;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks.ValueObjects;

public record RocketTaskType
{
    public string Type { get; init; }

    private RocketTaskType(string type) => Type = type;

    public static Result<RocketTaskType> Create(string? type)
    {
        if (string.IsNullOrEmpty(type))
            return RocketTaskErrors.CannotCreateWithEmptyType;

        return type switch
        {
            null => RocketTaskErrors.CannotCreateWithEmptyType,
            not null when string.IsNullOrWhiteSpace(type) =>
                RocketTaskErrors.CannotCreateWithEmptyType,
            not null when string.Equals("Одноразовая", type, StringComparison.OrdinalIgnoreCase) =>
                new RocketTaskType(type),
            not null when string.Equals("Многоразовая", type, StringComparison.OrdinalIgnoreCase) =>
                new RocketTaskType(type),
            _ => RocketTaskErrors.RocketTaskTypeIsNotAllowed,
        };
    }
}
