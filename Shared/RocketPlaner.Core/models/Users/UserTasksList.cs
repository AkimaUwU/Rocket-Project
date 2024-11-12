using System;
using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users;

public class UserTasksList : CustomList<RocketTask>
{
    internal UserTasksList() { }

    public override Result<RocketTask> Add(RocketTask item)
    {
        if (item == null)
            return new Error("Задача некорректна");

        if (Items.Any(x => x.Title == item.Title))
            return new Error("У вас уже есть задача с таким заголовком");

        Items.Add(item);
        return item;
    }

    public override Result<RocketTask> Find(Func<RocketTask, bool> predicate)
    {
        var task = Items.FirstOrDefault(predicate);
        if (task == null)
            return new Error("Задача не найдена");

        return task;
    }

    public override IReadOnlyList<RocketTask> GetAll() => Items;

    public override Result<RocketTask> Remove(RocketTask item)
    {
        Items.Remove(item);
        return item;
    }
}
