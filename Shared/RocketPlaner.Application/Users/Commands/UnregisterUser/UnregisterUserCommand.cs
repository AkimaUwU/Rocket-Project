using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Users.Commands.UnregisterUser;

public sealed record UnregisterUserCommand(long? UserTelegramId) : ICommand<User>;
