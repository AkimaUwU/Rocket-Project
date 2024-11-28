using RocketPlaner.Core.models.RocketTasks.Errors;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks.ValueObjects;

public readonly record struct RocketTaskFireDate
{
    public DateTime FireDate { get; init; }

    private RocketTaskFireDate(DateTime fireDate) => FireDate = fireDate;

    public static Result<RocketTaskFireDate> Create(DateTime? fireDate)
    {
        var current = DateTime.Now;
        return fireDate switch
        {
            null => RocketTaskErrors.CannotCreateWithEmptyDate,
            { } date when date < current => RocketTaskErrors.CannotCreateWithPastTime,
            _ => new RocketTaskFireDate(fireDate.Value),
        };
    }
}
