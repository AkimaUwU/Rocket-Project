using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Queries.GetUsersTask;

public record GetUsersTaskQuery(long? TelegramId) : IQuery<IReadOnlyList<RocketTask>>;
