using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Core.models.Users;

namespace RocketPlaner.TelegramBot;

public class TestEndpoint
{
    public TestEndpoint(ICommandDispatcher dispatcher, long telegramId)
    {
        var command = new RegisterUserCommand(telegramId);
        var result = dispatcher.Resolve<RegisterUserCommand, User>(command);
    }
}
