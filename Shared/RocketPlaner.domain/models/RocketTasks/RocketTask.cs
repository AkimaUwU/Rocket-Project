

using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;
using RocketPlaner.domain.models.Users;
using RocketPlaner.domain.Tools;

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

	public RocketTaskDestinationList Destinations { get; init; } = [];

	public string Title {get; private set;}

	/// <summary>
	/// Создания запланированной задачи со значениями
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="type">Тип задачи</param>
	/// <param name="notifyTime">Дата уведомления о задаче</param>
	private RocketTask(string message, User user, RocketTaskType type, DateTime notifyTime, string title) : base(Guid.NewGuid())
	{
		Owner = user;
		Message = message;
		Type = type;
		NotifyDate = notifyTime;
		CreatedDate = DateTime.Now;
		Title = title;
	}

	/// <summary>
	/// Фабричный метод создания задачи
	/// </summary>
	/// <param name="message">Сообщение уведомления</param>
	/// <param name="type">Тип созданной задачи</param>
	/// <param name="notifyTime">Дата уведомления задачи</param>
	/// <returns></returns>
	public static Resoult<RocketTask> Create(string message, string[] destinations, User user, RocketTaskType type, DateTime notifyTime,string title)
	{
		if (user == null)
			return new Error("Задача не может быть создана без пользователя");

		if (string.IsNullOrWhiteSpace(message))
			return new Error("Сообщение не может быть пустым");

		if (notifyTime < DateTime.Now)
			return new Error("Некорректная дата уведомления задачи. Дата не должна быть ранее текущей даты");
		
		if (notifyTime == DateTime.Now)
			return new Error("Некорректное время уведомления задачи. Время уведомления задачи не может быть текущим");

		RocketTask task = new RocketTask(message, user, type, notifyTime,title);

		foreach (string dest in destinations)
		{
			var Destini = RocketTaskDestination.Create(dest).Value;		
			task.Destinations.Add(Destini);
		}

		return task;
	}			



	/// <summary>
	/// Метод обновления времени для задачи
	/// </summary>
	/// <param name="updated">Новое время уведомления задачи</param>
	/// <returns></returns>
	public Resoult<RocketTask> UpdateNotificationDate(DateTime updated)
	{
		NotifyDate = updated;
		return this;
	}

	
}
