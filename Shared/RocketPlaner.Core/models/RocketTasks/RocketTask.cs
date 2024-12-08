using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTaskDestinations.Errors;
using RocketPlaner.Core.models.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks.Errors;
using RocketPlaner.Core.models.RocketTasks.Events;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks;

public class RocketTask : DomainAggregateRoot
{
    private readonly List<RocketTaskDestination> _destinations = [];

    private RocketTask() { }

    internal RocketTask(
        RocketTaskTitle title,
        RocketTaskMessage message,
        RocketTaskType type,
        RocketTaskFireDate fireDate,
        User owner
    )
        : this()
    {
        Title = title;
        Message = message;
        Owner = owner;
        Type = type;
        FireDate = fireDate;
    }

    public int Id { get; private set; }
    public RocketTaskTitle Title { get; private set; } = null!;
    public RocketTaskMessage Message { get; private set; } = null!;
    public RocketTaskType Type { get; private set; } = null!;
    public RocketTaskFireDate FireDate { get; private set; }
    public User Owner { get; private set; } = null!;

    public IReadOnlyList<RocketTaskDestination> Destinations => _destinations;

    public void UpdateNotificationDate(RocketTaskFireDate fireDate)
    {
        FireDate = fireDate;
        RaiseEvent(new RocketTaskFireDateUpdated(this));
    }

    public Result<RocketTaskDestination> AddDestination(DestinationChatId chatId)
    {
        if (OwnsDestination(d => d.ChatId == chatId))
            return RocketTaskErrors.OwnsDestinationAlready;

        var destination = new RocketTaskDestination(chatId, this);
        _destinations.Add(destination);
        RaiseEvent(new RocketTaskDestinationAddedToTask(this, destination));
        return destination;
    }

    public Result<RocketTaskDestination> RemoveDestination(RocketTaskDestination? destination)
    {
        if (destination is null)
            return RocketTaskErrors.DoesntOwnsDestinationChat;

        if (!OwnsDestination(d => d.ChatId == destination.ChatId))
            return RocketTaskErrors.DoesntOwnsDestinationChat;

        var isRemoved = _destinations.Remove(destination);
        if (!isRemoved)
            return RocketTaskErrors.DoesntOwnsDestinationChat;

        RaiseEvent(new RocketTaskDestinationRemoved(destination));
        return destination;
    }

    public Result<RocketTaskDestination> FindDestination(
        Func<RocketTaskDestination, bool> predicate
    )
    {
        var destination = _destinations.FirstOrDefault(predicate);
        return destination is null ? RocketTaskDestinationErrors.DestinationNotFound : destination;
    }

    private bool OwnsDestination(Func<RocketTaskDestination, bool> predicate) =>
        _destinations.Any(predicate);
}
