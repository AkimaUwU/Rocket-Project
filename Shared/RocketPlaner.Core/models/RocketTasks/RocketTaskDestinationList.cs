using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks;

public class RocketTaskDestinationList : CustomList<RocketTaskDestination>
{
    internal RocketTaskDestinationList() { }

    public override Result<RocketTaskDestination> Add(RocketTaskDestination item)
    {
        if (item == null)
            return new Error("Место отправки некорректно");

        if (Items.Any(x => x.ChatId == item.ChatId))
            return new Error("Этот чат уже существует в этой задаче");

        Items.Add(item);
        return item;
    }

    public override Result<RocketTaskDestination> Find(Func<RocketTaskDestination, bool> predicate)
    {
        var destination = Items.FirstOrDefault(predicate);
        if (destination == null)
            return new Error("Чат не найден");

        return destination;
    }

    public override IReadOnlyList<RocketTaskDestination> GetAll() => Items;

    public override Result<RocketTaskDestination> Remove(RocketTaskDestination item)
    {
        Items.Remove(item);
        return item;
    }
}
