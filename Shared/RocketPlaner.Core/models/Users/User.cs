using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.Errors;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users.Events;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users;

public sealed class User : DomainAggregateRoot
{
    private readonly List<RocketTask> _tasks = [];

    private User() { } // EF core constructor

    public User(UserTelegramId telegramId)
        : this()
    {
        TelegramId = telegramId;
        RaiseEvent(new UserCreated(this));
    }

    public UserTelegramId TelegramId { get; init; }
    public IReadOnlyList<RocketTask> Tasks => _tasks;

    public Result<RocketTask> RegisterRocketTask(
        RocketTaskTitle title,
        RocketTaskMessage message,
        RocketTaskType type,
        RocketTaskFireDate fireDate
    )
    {
        if (DoesUserHaveTask(t => t.Title == title))
            return RocketTaskErrors.UserOwnsTaskAlready;

        var task = new RocketTask(title, message, type, fireDate, this);
        _tasks.Add(task);
        RaiseEvent(new UserAddedTask(this, task));
        return task;
    }

    public Result<RocketTask> UnregisterRocketTask(RocketTask? task)
    {
        if (task is null)
            return RocketTaskErrors.RocketTaskIsNull;

        if (!DoesUserHaveTask(t => t.Id == task.Id))
            return RocketTaskErrors.UserDoesntOwnsTask;

        var isRemoved = _tasks.Remove(task);
        if (!isRemoved)
            return RocketTaskErrors.UserDoesntOwnsTask;

        RaiseEvent(new UserRemovedTask(this, task));
        return task;
    }

    public Result<RocketTask> FindRocketTask(Func<RocketTask, bool> predicate)
    {
        var requested = _tasks.FirstOrDefault(predicate);
        return requested is null ? RocketTaskErrors.TaskWasNotFoundInUserTasksList : requested;
    }

    private bool DoesUserHaveTask(Func<RocketTask, bool> predicate) => _tasks.Any(predicate);
}
