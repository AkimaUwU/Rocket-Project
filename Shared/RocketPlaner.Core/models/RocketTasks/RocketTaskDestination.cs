using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.Tools;

namespace RocketPlaner.Core.models.RocketTasks;

public class RocketTaskDestination : DomainEntity
{
    /// <summary>
    /// ИД Чата куда задача будет отправлена
    /// </summary>
    public string ChatId { get; init; }

    /// <summary>
    /// Конструктор создания места отправления задачи
    /// </summary>
    /// <param name="id">ИД экземпляра класса</param>
    /// <param name="chatId">ИД чата</param>
    private RocketTaskDestination(Guid id, string chatId)
        : base(id) => ChatId = chatId;

    /// <summary>
    /// Статичный фабричный метод создания экземпляра класса RocketTaskDestination
    /// </summary>
    /// <param name="chatId">ID чата</param>
    /// <returns>Результат создания класса RocketTaskDestination. Успешный/Ошибка</returns>
    public static Result<RocketTaskDestination> Create(string chatId)
    {
        if (string.IsNullOrWhiteSpace(chatId))
            return new Error("ID чата не было указано");

        return new RocketTaskDestination(Guid.NewGuid(), chatId);
    }
}
