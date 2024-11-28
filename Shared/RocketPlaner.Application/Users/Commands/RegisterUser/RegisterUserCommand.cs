using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand(long telegramId) : ICommand<User>
{
    public long? TelegramId { get; init; } = telegramId;
}
