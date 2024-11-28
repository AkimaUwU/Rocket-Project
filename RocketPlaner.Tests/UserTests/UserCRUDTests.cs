using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Events;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Application.Users.Commands.UnregisterUser;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.Events;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

namespace RocketPlaner.Tests.UserTests;

[TestFixture]
public sealed class UserCRUDTests
{
    private readonly IServiceCollection _services;

    public UserCRUDTests()
    {
        _services = new ServiceCollection();

        _services.AddScoped<IUsersDataBase, UsersDatabase>();

        _services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        _services.AddScoped<DomainEventDispatcher, DomainEventDispatcher>();

        _services.AddTransient<
            ICommandHandler<RegisterUserCommand, User>,
            RegisterUserCommandHandler
        >();
        _services.AddTransient<
            ICommandHandler<UnregisterUserCommand, User>,
            UnregisterUserCommandHandler
        >();

        _services.AddTransient<IDomainEventHandler<UserCreated>, RegisterUserEventHandler>();
    }

    [Test, Order(1)]
    public async Task RegisterUser()
    {
        var telegramId = 123;
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
