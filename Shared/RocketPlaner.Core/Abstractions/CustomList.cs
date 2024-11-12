using System.Collections;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.Abstractions;

public abstract class CustomList<T> : IList<T>
{
    protected readonly List<T> Items = [];

    // Группа методов интерфейса IList<T>
    T IList<T>.this[int index]
    {
        get => Items[index];
        set => Items[index] = value;
    }

    int ICollection<T>.Count => Items.Count;
    bool ICollection<T>.IsReadOnly => true;

    void ICollection<T>.Add(T item) => Items.Add(item);

    void ICollection<T>.Clear() => Items.Clear();

    bool ICollection<T>.Contains(T item) => Items.Contains(item);

    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

    int IList<T>.IndexOf(T item) => Items.IndexOf(item);

    void IList<T>.Insert(int index, T item) => Items.Insert(index, item);

    bool ICollection<T>.Remove(T item) => Items.Remove(item);

    void IList<T>.RemoveAt(int index) => Items.RemoveAt(index);

    // Группа методов-контрактов для реализации пользовательских коллекций.
    public abstract Result<T> Add(T item);
    public abstract Result<T> Remove(T item);
    public abstract Result<T> Find(Func<T, bool> predicate);
    public abstract IReadOnlyList<T> GetAll();
}
