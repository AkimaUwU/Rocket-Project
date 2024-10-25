using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.models.RocketTasks;
using CSharpFunctionalExtensions;

namespace RocketPlaner.domain.models.Users;

public sealed class User : DomainAggregateRoot
{
	private readonly List<RocketTask> _tasks = [];

	public string TelegramId { get; init; }

    private User() : base(Guid.Empty)
    {
		TelegramId = string.Empty;
    }

	private User(string telegramId, Guid id) : base(id)
	{
		TelegramId = telegramId;
	}

	public IReadOnlyCollection<RocketTask> Tasks => _tasks.ToList();

	public static User Default => new User();

	public static Result<User> Create(string telegramId)
	{
		if (string.IsNullOrWhiteSpace(telegramId))
			return Result.Failure<User>("Нельзя создать пользователя без идентификатора пользователя телеграмм");

		return new User(telegramId, Guid.NewGuid());
	}

	public Result AddTask(RocketTask task)
	{
		if (_tasks.Any(t => t.Id == task.Id))
			return Result.Failure("Такая задача уже есть"); 
		
		if (_tasks.Any(t => 
			t.Type.Type == task.Type.Type &&
			t.Message == task.Message && 
			t.NotifyDate.Date == task.NotifyDate.Date &&
			t.Destinations.SequenceEqual(task.Destinations)))
			return Result.Failure("Такая задача уже есть"); 
		
		_tasks.Add(task);									
		return Result.Success();
	}

	public Result Remove(Guid id)
	{
		RocketTask? requested =	_tasks.FirstOrDefault(t => t.Id == id);
		if (requested == null)
			return Result.Failure("Задача не была найдена");
		
		_tasks.Remove(requested);
		return Result.Success();
	}	
}