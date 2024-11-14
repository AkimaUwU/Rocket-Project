using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Contracts.DataBaseContracts;

/// <summary>
/// Интерфейс таблицы пользователей в базе данных
/// </summary>
public interface IUsersDataBase
{
    /// <summary>
    /// Метод добавления пользователя в таблицу базы данных
    /// </summary>
    /// <param name="user">Модель пользователя</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task AddUser(User user);

    /// <summary>
    /// Метод удаления пользователя из таблицы базы данных
    /// </summary>
    /// <param name="user">Модель пользователя</param>
    /// <returns>Task выполнение асинхронной операции</returns>
    Task RemoveUser(User user);

    /// <summary>
    /// Метод получения пользователя по ИД
    /// </summary>
    /// <param name="telegramId">Идентификатор ИД телеграмма пользователя</param>
    /// <returns>Модель пользователя</returns>
    Task<User?> GetUser(long telegramId);
}
