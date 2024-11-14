using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public class GetUsersTaskQuery(long telegramId) : IQuery<IReadOnlyList<RocketTask>>
{
    public long TelegramId { get; init; } = telegramId;
}
