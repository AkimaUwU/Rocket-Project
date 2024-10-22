using CSharpFunctionalExtensions;

namespace RocketPlaner.Tests.Models;

public abstract class TestSample<T>
{
	public abstract Result<T> Invoke();
}
