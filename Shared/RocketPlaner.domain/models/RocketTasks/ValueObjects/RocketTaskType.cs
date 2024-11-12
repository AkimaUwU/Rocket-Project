

using RocketPlaner.domain.Abstractions;

namespace RocketPlaner.domain.models.RocketTasks.ValueObjects;

// Объект - значение. Тип запланированной задачи.
public abstract record RocketTaskType (string Type);
	
public record OneLife (): RocketTaskType ("Одноразовая");
public record NoOneLife(): RocketTaskType ("Повторяющаяся");
