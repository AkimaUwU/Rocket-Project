using CSharpFunctionalExtensions;

using RocketPlaner.domain.Abstractions;

namespace RocketPlaner.domain.models.RocketTasks.ValueObjects;

public class RocketTaskType : DomainValueObject
{
	public string Type { get; private set; }

	private RocketTaskType()
	{
		Type = string.Empty;
	}

	private RocketTaskType(string type)
	{
		Type = type;
	}

	public static Result<RocketTaskType> Create(string type)
	{
		if (string.IsNullOrWhiteSpace(type))
			return Result.Failure<RocketTaskType>("Тип задачи не указан");

		if (_types.Any(x => x.Type == type) == false)
			return Result.Failure<RocketTaskType>($"Недопустимый тип задачи. Допустимы: {_types[0].Type} {_types[1].Type}");

		return new RocketTaskType(type);
	}

	public override IEnumerable<object> GetEqualityComponents()
	{
		yield return Type;
	}

	private static RocketTaskType[] _types = [
		new("Повторяющаяся"),
		new("Одноразовая")];

	public static RocketTaskType Defoult => new RocketTaskType();
}
