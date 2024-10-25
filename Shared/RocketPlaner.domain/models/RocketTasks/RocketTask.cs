using CSharpFunctionalExtensions;

using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;
using RocketPlaner.domain.models.Users;

namespace RocketPlaner.domain.models.RocketTasks;

/// <summary>
/// Доменная сущность - запланированная задача
/// </summary>
public class RocketTask : DomainAggregateRoot
{	
	
	/// <summary>
	/// Создатель задачи
	/// </summary>
	public User Owner { get; private set; }

	/// <summary>
	/// Список мест назначения задачи (формально чаты куда отправлять). Закрытый.
	/// </summary>
	private List<RocketTaskDestination> _destinations = [];

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
	/// Список мест назначения задачи открытый только для чтения.
	/// </summary>

	public IReadOnlyCollection<RocketTaskDestination> Destinations => _destinations.ToList();

	/// <summary>
	/// Создание запланированной задачи без значений.
	/// </summary>
	private RocketTask() : base(Guid.Empty)
	{
		Owner = User.Default;
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
	private RocketTask(string message, User user, RocketTaskType type, DateTime notifyTime) : base(Guid.NewGuid())
	{
		Owner = user;
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
	public static Result<RocketTask> Create(string message, string[] destinations, User user, RocketTaskType type, DateTime notifyTime)
	{
		if (user == null)
			return Result.Failure<RocketTask>("Задача не может быть создана без пользователя");

		if (string.IsNullOrWhiteSpace(message))
			return Result.Failure<RocketTask>("Сообщение не может быть пустым");

		if (notifyTime < DateTime.Now)
			return Result.Failure<RocketTask>("Некорректная дата уведомления задачи. Дата не должна быть ранее текущей даты");
		
		if (notifyTime == DateTime.Now)
			return Result.Failure<RocketTask>("Некорректное время уведомления задачи. Время уведомления задачи не может быть текущим");

		RocketTask task = new RocketTask(message, user, type, notifyTime);

		foreach (string dest in destinations)
		{
			Result insert = task.AddDestination(dest);			
		}

		return task;
	}			

	// to do добавить метод добавления мест уведомления.

	/// <summary>
	/// Фабричный метод создания одноразовой задачи
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="notifyTime">Дата уведомления</param>
	/// <returns></returns>
	public static Result<RocketTask> CreateWithOneLife(string message, User user, DateTime notifyTime)
	{
		RocketTaskType type = RocketTaskType.Create("Одноразовая").Value;
		return new RocketTask(message, user, type, notifyTime);
	}

	/// <summary>
	/// Фабричный метод создания повторяющейся задачи
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="notifyTime">Дата уведомления</param>
	/// <returns></returns>
	public static Result<RocketTask> CreateRepetitive(string message, User user, DateTime notifyTime)
	{
		RocketTaskType type = RocketTaskType.Create("Повторяющаяся").Value;
		return new RocketTask(message, user, type, notifyTime);
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

	/// <summary>
	/// Метод добавления задачи
	/// </summary>
	/// <param name="chatId">Идентификатор чата</param>
	/// <returns></returns>
	public Result AddDestination(string chatId)
	{
		Result<RocketTaskDestination> destination = RocketTaskDestination.Create(chatId);
		if (destination.IsFailure)
			return Result.Failure("Невозможно добавить такую задачу");
		
		if (_destinations.Any(d => d.ChatId == chatId ))
			return Result.Failure("Место назначения уведомления уже существует для этой задачи");
		
		_destinations.Add(destination.Value);
		return Result.Success();
	}

	/// <summary>
	/// Метод удаления места задачи
	/// </summary>
	/// <param name="chatId"></param>
	/// <returns>Результат удаления задачи. Ошибка или успех.</returns>
	public Result RemoveDestion(string chatId)
	{
		if (_destinations.Count == 1)
			return Result.Failure("Задача не может существовать без места уведомления. Удалить текущую задачу?");

		RocketTaskDestination? requested = _destinations.FirstOrDefault(d => d.ChatId == chatId);
		if (requested == null)
			return Result.Failure("Задача не найдена");
		
		_destinations.Remove(requested);
		return Result.Success();
	}
}
