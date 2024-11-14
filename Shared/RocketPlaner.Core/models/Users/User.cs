using System.Text;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.Users;

/// <summary>
/// Сущность - пользователь
/// </summary>
public sealed class User : DomainAggregateRoot
{
    /// <summary>
    /// Список задач пользователя
    /// </summary>
    public UserTasksList Tasks { get; init; } = new UserTasksList();

    /// <summary>
    /// ИД телеграмма пользователя
    /// </summary>
    public long TelegramId { get; init; }

    /// <summary>
    /// Закрытый конструктор создания экземпляра класса пользователь
    /// </summary>
    /// <param name="telegramId">ИД телеграмма</param>
    /// <param name="id">ИД пользователя</param>
    private User(long telegramId, Guid id)
        : base(id) => TelegramId = telegramId;

    /// <summary>
    /// Фабричный метод создания экземпляра класса пользователя
    /// </summary>
    /// <param name="telegramId">ИД телеграмма</param>
    /// <returns>Результат создания сущности пользователь.</returns>
    public static Result<User> Create(long telegramId)
    {
        if (telegramId <= 0)
            return new Error("ID телеграмма пользователя некорректно");

        return new User(telegramId, Guid.NewGuid());
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"ИД пользователя: {Id}");
        builder.AppendLine($"ИД телеграмма пользователя: {TelegramId}");
        return builder.ToString();
    }
}
