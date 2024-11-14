using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.Abstractions;

public abstract class CustomList<T>
{
    protected readonly List<T> Items = [];

    // Группа методов-контрактов для реализации пользовательских коллекций.
    public abstract Result<T> Add(T item);
    public abstract Result<T> Remove(T item);
    public abstract Result<T> Find(Func<T, bool> predicate);
    public abstract IReadOnlyList<T> GetAll();

    // Группа общих методов для пользовательских коллекций
    protected T? TwoWaySearch(Func<T, bool> predicate)
    {
        if (Items.Count == 0)
            return default;

        if (Items.Count == 1 && predicate(Items[0]))
            return Items[0];

        int leftIndex = 0;
        int rightIndex = Items.Count - 1;
        while (leftIndex <= rightIndex)
        {
            if (predicate(Items[leftIndex]))
                return Items[leftIndex];

            if (predicate(Items[rightIndex]))
                return Items[rightIndex];

            leftIndex++;
            rightIndex--;
        }
        return default;
    }
}
