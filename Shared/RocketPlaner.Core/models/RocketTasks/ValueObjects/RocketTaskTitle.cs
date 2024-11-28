using RocketPlaner.Core.models.RocketTasks.Errors;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks.ValueObjects;

public record RocketTaskTitle
{
    public const int MaxTitleLength = 128;

    public string Title { get; init; }

    private RocketTaskTitle(string title) => Title = title;

    public static Result<RocketTaskTitle> Create(string? title)
    {
        return title switch
        {
            null => RocketTaskErrors.CannotHaveEmptyTitle,
            not null when string.IsNullOrWhiteSpace(title) => RocketTaskErrors.CannotHaveEmptyTitle,
            { Length: > MaxTitleLength } => RocketTaskErrors.MoreThanMaxSize,
            _ => new RocketTaskTitle(title),
        };
    }
}
