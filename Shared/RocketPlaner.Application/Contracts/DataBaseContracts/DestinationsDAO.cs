using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Модель для таблицы базы данных - места отправки задач
/// </summary>
public class DestinationsDao
{
    /// <summary>
    /// ИД места отправки задачи
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ИД телеграмм чата
    /// </summary>
    public string ChatId { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное поле - задача для связи 1 * М
    /// </summary>
    public TasksDao Task { get; set; } = null!;

    /// <summary>
    /// Внешний ключ к таблице задач
    /// </summary>
    public Guid TaskId { get; set; }
}

public static class DestinationsDaoExtensions
{
    public static RocketTaskDestination ToRocketTaskDestination(
        this DestinationsDao destinationsDao,
        RocketTask task
    )
    {
        var destination = RocketTaskDestination
            .Create(destinationsDao.Id, task, destinationsDao.ChatId)
            .Value;
        return destination;
    }

    public static DestinationsDao ToDestinationsDao(
        this RocketTaskDestination destination,
        TasksDao task
    )
    {
        var destinationsDao = new DestinationsDao()
        {
            Id = destination.Id,
            ChatId = destination.ChatId,
            TaskId = destination.Task.Id,
            Task = task,
        };
        return destinationsDao;
    }
}
