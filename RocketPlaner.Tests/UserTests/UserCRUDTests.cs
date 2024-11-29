using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.DependencyInjection;
using RocketPlaner.Application.RocketTasks.DependencyInjection;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Application.Users.Commands.UnregisterUser;
using RocketPlaner.Application.Users.DependencyInjection;
using RocketPlaner.Core.models.Users;
using RocketPlaner.DataAccess.DependencyInjection;

namespace RocketPlaner.Tests.UserTests;

[TestFixture]
[Category("UserTests")]
public sealed class UserCrudTests
{
    private readonly IServiceCollection _services;

    public UserCrudTests()
    {
        _services = new ServiceCollection();
        _services
            .AddApplicationCommonServices()
            .AddDataAccessServices()
            .AddUserApplicationServices()
            .AddRocketTasksServices();
    }

    [Test, Order(1)]
    public async Task RegisterUser()
    {
        const int telegramId = 123;
        var command = new RegisterUserCommand(telegramId);
        await using var provider = _services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();
        var registeredUser = await dispatcher.Resolve<RegisterUserCommand, User>(command);

        var users = provider.GetRequiredService<IUsersDataBase>();
        var check = await users.GetUser(registeredUser.Value.TelegramId);

        Assert.That(check, Is.Not.Null);
    }

    [Test, Order(2)]
    public async Task UnregisterUser()
    {
        var telegramId = 123;
        var command = new UnregisterUserCommand(telegramId);
        await using var provider = _services.BuildServiceProvider();
        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();
        var unregisteredUser = await dispatcher.Resolve<UnregisterUserCommand, User>(command);

        var users = provider.GetRequiredService<IUsersDataBase>();
        var check = await users.GetUser(unregisteredUser.Value.TelegramId);

        Assert.That(check, Is.Null);
    }
}
