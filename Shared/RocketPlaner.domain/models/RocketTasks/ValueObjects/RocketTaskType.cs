using CSharpFunctionalExtensions;

using RocketPlaner.domain.Abstractions;

namespace RocketPlaner.domain.models.RocketTasks.ValueObjects;

// Объект - значение. Тип запланированной задачи.
public class RocketTaskType : DomainValueObject
{
	// Допустимые типы запланированной задачи: Повторяющаяся и Одноразовая.
	private static RocketTaskType[] _types = [
		new("Повторяющаяся"),
		new("Одноразовая")];

	// Поле - значение типа запланированной задачи.
	public string Type { get; private set; }

	// Конструктор создания пустого объекта (с пустым значением)
	private RocketTaskType() => Type = string.Empty;

	// Конструктор создания типа задачи.
	private RocketTaskType(string type) => Type = type;

	// Пустой вариант типа задачи.
	public static RocketTaskType Default => new RocketTaskType();

	// Фабричный метод создания типа задачи с валидацией.
	public static Result<RocketTaskType> Create(string type)
	{
		if (string.IsNullOrWhiteSpace(type))
			return Result.Failure<RocketTaskType>("Тип задачи не указан");

		if (_types.Any(x => x.Type == type) == false)
			return Result.Failure<RocketTaskType>($"Недопустимый тип задачи. Допустимы: {_types[0].Type} {_types[1].Type}");

		return new RocketTaskType(type);
	}

	// Реализация DomainValueObject класса для сравнения типов задач.
	public override IEnumerable<object> GetEqualityComponents()
	{
		yield return Type;
	}
}
