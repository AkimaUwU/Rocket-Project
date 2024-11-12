using System;
using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.Tools;

namespace RocketPlaner.domain.models.RocketTasks;

public class RocketTaskDestinationList : CustomList<RocketTaskDestination>
{
    public override Resoult<RocketTaskDestination> Add(RocketTaskDestination item)
    {
        if(Items.Any(x=>x.ChatId == item.ChatId)) 
        {
            return new Error ("Этот чат уже существует в этой задаче");
        }
        Items.Add(item);
        return item;
    }

    public override Resoult<RocketTaskDestination> Find(Func<RocketTaskDestination, bool> Predicate)
    {
        var Destinations = Items.FirstOrDefault(Predicate);
        if (Destinations == null)
        {
            return new Error ("Чат не найден");
        }
        return Destinations;
    }

    public override IReadOnlyList<RocketTaskDestination> GetAll()
    {
        return Items;
    }

    public override Resoult<RocketTaskDestination> Remove(RocketTaskDestination item)
    {
        Items.Remove(item);
        return item;
    }
}

