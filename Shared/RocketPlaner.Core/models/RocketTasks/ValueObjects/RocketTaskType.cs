namespace RocketPlaner.Core.models.RocketTasks.ValueObjects;

// Объект - значение. Тип запланированной задачи.
public abstract record RocketTaskType(string Type);

/// <summary>
/// Одноразовая задача
/// </summary>
public record OneLife() : RocketTaskType("Одноразовая");

/// <summary>
/// Многоразовая задача
/// </summary>
public record NoOneLife() : RocketTaskType("Повторяющаяся");
