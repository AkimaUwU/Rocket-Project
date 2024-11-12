using System;
using System.Security.Cryptography.X509Certificates;

namespace RocketPlaner.domain.Tools;

public class Resoult <T> 
{
    public Error Error { get; init; }
    public T Value { get; init; }

    public bool IsError { get; init; }
    public bool IsOk { get; init; }

    private Resoult(Error error)
    {
        IsError = true;
        IsOk = false;
        Error = error;
        Value = default!;
    }

    private Resoult(T value)
    {
        IsError = false;
        IsOk = true;
        Error = Error.None;
        Value = value;
    }
    public static Resoult <T> Succes (T value)
    {
        return new Resoult<T> (value);
    }
    public static Resoult<T> Fail (Error error)
    {
        return new Resoult <T> (error);
    }
     public static implicit operator Resoult<T> (T value)
     {
        return Succes(value);
     }
     public static implicit operator Resoult<T> (Error error)
     {
        return Fail(error);
     }
}
