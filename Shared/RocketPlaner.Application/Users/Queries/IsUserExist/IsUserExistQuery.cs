using RocketPlaner.Application.Contracts.Operations;

namespace RocketPlaner.Application.Users.Queries.IsUserExist;

public sealed record IsUserExistQuery(long? TelegramId) : IQuery<bool>;
