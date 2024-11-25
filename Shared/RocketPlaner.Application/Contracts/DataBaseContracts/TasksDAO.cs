using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Модель таблицы задача в базе данных
/// </summary>
public class TasksDao
{
    /// <summary>
    /// Ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внешний ключ на таблицу пользователя
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Навигационное свойство пользователь для связи 1 * М
    /// </summary>
    public UsersDao Owner { get; set; } = null!;

    /// <summary>
    /// Дата создания задачи
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Дата уведомления о задаче
    /// </summary>
    public DateTime NotifyDate { get; set; }

    /// <summary>
    /// Тип задачи
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Сообщение задачи
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Заголовок задачи
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное свойство список мест отправки задачи для связи 1 * М
    /// </summary>
    public List<DestinationsDao> Destinations { get; set; } = [];
}

public static class TasksDaoExtensions
{
    public static RocketTask ToRocketTask(this TasksDao tasksDao, User user)
    {
        RocketTask task = RocketTask.Create(
            tasksDao.Id,
            tasksDao.Message,
            user,
            tasksDao.Type,
            tasksDao.NotifyDate,
            tasksDao.Title
        );
        foreach (DestinationsDao destinationsDao in tasksDao.Destinations)
        {
            task.Destinations.Add(destinationsDao.ToRocketTaskDestination(task));
        }
        return task;
    }

    public static TasksDao ToRocketTaskDao(this RocketTask task, UsersDao user)
    {
        var tasksDao = new TasksDao()
        {
            Id = task.Id,
            CreateDate = task.CreatedDate,
            NotifyDate = task.NotifyDate,
            Message = task.Message,
            Owner = user,
            OwnerId = user.Id,
            Title = task.Title,
        };
        var destinations = task.Destinations.GetAll().ToArray();
        var destinationsDao = destinations
            .Select(dest => dest.ToDestinationsDao(tasksDao))
            .ToList();
        tasksDao.Destinations = destinationsDao;
        return tasksDao;
    }
}
