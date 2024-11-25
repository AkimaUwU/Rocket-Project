using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Модель таблицы пользователи в базе данных
/// </summary>
public sealed class UsersDao
{
    /// <summary>
    /// Ключ
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ИД телеграмма пользователя
    /// </summary>
    public long TelegramId { get; set; }

    /// <summary>
    /// Навигационное свойство список задач для связи 1 * М
    /// </summary>
    public List<TasksDao> Tasks { get; set; } = [];
}

public static class UsersDaoExtensions
{
    public static User ToUser(this UsersDao usersDao)
    {
        User user = User.Create(usersDao.Id, usersDao.TelegramId);
        foreach (TasksDao tasksDao in usersDao.Tasks)
        {
            user.Tasks.Add(tasksDao.ToRocketTask(user));
        }

        return user;
    }

    public static UsersDao ToUsersDao(this User user)
    {
        var usersDao = new UsersDao() { Id = user.Id, TelegramId = user.TelegramId };
        var tasks = user.Tasks.GetAll().ToArray();
        var tasksDaos = tasks.Select(task => task.ToRocketTaskDao(usersDao)).ToList();
        usersDao.Tasks = tasksDaos;
        return usersDao;
    }
}
