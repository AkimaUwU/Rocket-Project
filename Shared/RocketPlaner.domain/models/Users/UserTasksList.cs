using System;
using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.models.RocketTasks;
using RocketPlaner.domain.Tools;

namespace RocketPlaner.domain.models.Users;

public class UserTasksList : CustomList<RocketTask>
{
    public override Resoult<RocketTask> Add(RocketTask item)
    {
        if (Items.Any(x=>x.Title == item.Title))
        {
            return new Error ("У вас уже есть задача с таким заголовком");
        }
        Items.Add(item);
        return item;
    }

    public override Resoult<RocketTask> Find(Func<RocketTask, bool> Predicate)
    {
        var task = Items.FirstOrDefault(Predicate);
        if (task == null)
        {
            return new Error ("Задача не найдена");
        }
        return task;
    }

    public override IReadOnlyList<RocketTask> GetAll()
    {
        return Items;
    }

    public override Resoult<RocketTask> Remove(RocketTask item)
    {
        Items.Remove(item);
        return item;
    }
}
