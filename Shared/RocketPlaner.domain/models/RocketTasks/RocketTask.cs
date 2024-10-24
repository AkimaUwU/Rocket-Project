using CSharpFunctionalExtensions;

using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;

namespace RocketPlaner.domain.models.RocketTasks;

/// <summary>
/// Доменная сущность - запланированная задача
/// </summary>
public class RocketTask : DomainEntity
{
	/// <summary>
	/// Дата создания запланиированной задачи
	/// </summary>
	public DateTime CreatedDate { get; init; }

	/// <summary>
	/// Дата уведомления пользователя о запланированной задаче
	/// </summary>
	public DateTime NotifyDate { get; private set; }

	/// <summary>
	/// Сообщение уведомления
	/// </summary>
	public string Message { get; private set; }

	/// <summary>
	/// Тип запланированной задачи
	/// </summary>
	public RocketTaskType Type { get; private set; }

	/// <summary>
	/// Создание запланированной задачи без значений.
	/// </summary>
	private RocketTask() : base(Guid.Empty)
	{
		Message = string.Empty;
		Type = RocketTaskType.Default;
		NotifyDate = DateTime.MinValue;
		CreatedDate = DateTime.MinValue;
	}

	/// <summary>
	/// Создания запланированной задачи со значениями
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="type">Тип задачи</param>
	/// <param name="notifyTime">Дата уведомления о задаче</param>
	private RocketTask(string message, RocketTaskType type, DateTime notifyTime) : base(Guid.NewGuid())
	{
		Message = message;
		Type = type;
		NotifyDate = notifyTime;
		CreatedDate = DateTime.Now;
	}

	/// <summary>
	/// Фабричный метод создания задачи
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="type">Тип созданной задачи</param>
	/// <param name="notifyTime">Дата уведомления задачи</param>
	/// <returns></returns>
	public static Result<RocketTask> Create(string message, RocketTaskType type, DateTime notifyTime) => new RocketTask(message, type, notifyTime);

	/// <summary>
	/// Фабричный метод создания одноразовой задачи
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="notifyTime">Дата уведомления</param>
	/// <returns></returns>
	public static Result<RocketTask> CreateWithOneLife(string message, DateTime notifyTime)
	{
		RocketTaskType type = RocketTaskType.Create("Одноразовая").Value;
		return new RocketTask(message, type, notifyTime);
	}

	/// <summary>
	/// Фабричный метод создания повторяющейся задачи
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="notifyTime">Дата уведомления</param>
	/// <returns></returns>
	public static Result<RocketTask> CreateRepetitive(string message, DateTime notifyTime)
	{
		RocketTaskType type = RocketTaskType.Create("Повторяющаяся").Value;
		return new RocketTask(message, type, notifyTime);
	}

	/// <summary>
	/// Метод обновления времени для задачи
	/// </summary>
	/// <param name="updated">Новое время уведомления задачи</param>
	/// <returns></returns>
	public Result UpdateNotificationDate(DateTime updated)
	{
		NotifyDate = updated;
		return Result.Success();
	}
}
