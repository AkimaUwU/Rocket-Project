using System;
using System.Collections;
using RocketPlaner.domain.Tools;

namespace RocketPlaner.domain.Abstractions;

public abstract class CustomList<T> : IList<T>
{
    protected readonly List<T> Items = [];
    T IList<T>.this[int index] { get => Items[index]; set => Items[index]=value; }

    int ICollection<T>.Count => Items.Count;

    bool ICollection<T>.IsReadOnly => true;

    void ICollection<T>.Add(T item)
    {
        Items.Add(item);   
    }

    void ICollection<T>.Clear()
    {
        Items.Clear();
    }

    bool ICollection<T>.Contains(T item)
    {
       return Items.Contains(item);
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        Items.CopyTo(array, arrayIndex);
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    int IList<T>.IndexOf(T item)
    {
        return Items.IndexOf(item);
    }

    void IList<T>.Insert(int index, T item)
    {
        Items.Insert(index, item);
    }

    bool ICollection<T>.Remove(T item)
    {
        return Items.Remove(item);
    }

    void IList<T>.RemoveAt(int index)
    {
    
        Items.RemoveAt(index);
    }

    public abstract Resoult <T> Add (T item);
    public abstract Resoult <T> Remove (T item);

    public abstract Resoult <T> Find (Func<T,bool> Predicate);

    public abstract IReadOnlyList<T> GetAll();

}
