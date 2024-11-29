using Microsoft.Extensions.DependencyInjection;
using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Application.DependencyInjection;
using RocketPlaner.Application.RocketTasks.DependencyInjection;
using RocketPlaner.Application.Users.Commands.AddTaskForUsers;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Application.Users.Commands.UnregisterUser;
using RocketPlaner.Application.Users.Commands.UpdateTaskDate;
using RocketPlaner.Application.Users.DependencyInjection;
using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.models.Users.ValueObjects;
using RocketPlaner.DataAccess.DependencyInjection;

namespace RocketPlaner.Tests.UserInteractionWithTasksTests;

[TestFixture]
[Category("UserInteractionWithTasksTests")]
public sealed class UserInteractionWithTaskTest
{
    private readonly IServiceCollection _services;

    public UserInteractionWithTaskTest()
    {
        _services = new ServiceCollection();
        _services
            .AddApplicationCommonServices()
            .AddDataAccessServices()
            .AddUserApplicationServices()
            .AddRocketTasksServices();
    }

    [Test, Order(1)]
    public async Task ACreateUserTest()
    {
        var serviceProvider = _services.BuildServiceProvider();
        const long telegramId = 123;
        var registerUserCommand = new RegisterUserCommand(telegramId);
        var dispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
        var registeredUserResult = await dispatcher.Resolve<RegisterUserCommand, User>(
            registerUserCommand
        );
        var usersDb = serviceProvider.GetRequiredService<IUsersDataBase>();
        var userCheck = await usersDb.GetUser(registeredUserResult.Value.TelegramId);
        Assert.That(userCheck, Is.Not.Null);
    }

    [Test, Order(2)]
    public async Task BCreateTaskTest()
    {
        var serviceProvider = _services.BuildServiceProvider();
        string title = "Some title";
        string message = "Some message";
        string type = "Одноразовая";
        DateTime fireDate = DateTime.Now.AddHours(1);
        const long telegramId = 123;

        var addTaskCommand = new AddTaskForUsersCommand(title, message, type, fireDate, telegramId);
        var dispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
        var createdTask = await dispatcher.Resolve<AddTaskForUsersCommand, RocketTask>(
            addTaskCommand
        );
        var usersDb = serviceProvider.GetRequiredService<IUsersDataBase>();
        var userCheck = await usersDb.GetUser(UserTelegramId.Create(telegramId).Value);

        var createdTaskVerify = userCheck.FindRocketTask(t => t.Title == createdTask.Value.Title);
        Assert.That(createdTaskVerify.IsError, Is.False);
    }

    [Test, Order(3)]
    public async Task CUpdateTaskDateTest()
    {
        var serviceProvider = _services.BuildServiceProvider();
        string title = "Some title";
        string message = "Some message";
        string type = "Одноразовая";
        DateTime fireDate = DateTime.Now.AddHours(1);
        const long telegramId = 123;
        var addTaskCommand = new AddTaskForUsersCommand(title, message, type, fireDate, telegramId);
        var updateTaskDateCommand = new UpdateTaskDateCommand(telegramId, title, fireDate);
        var dispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
        var createdTask = await dispatcher.Resolve<AddTaskForUsersCommand, RocketTask>(
            addTaskCommand
        );
        var updatedTaskValue = await dispatcher.Resolve<UpdateTaskDateCommand, RocketTask>(
            updateTaskDateCommand
        );

        var usersDb = serviceProvider.GetRequiredService<IUsersDataBase>();
        var userCheck = await usersDb.GetUser(UserTelegramId.Create(telegramId).Value);
        var updatedTaskVerify = userCheck.FindRocketTask(t =>
            t.Title == updatedTaskValue.Value.Title
        );
        Assert.That(
            updatedTaskVerify.Value.FireDate.FireDate,
            Is.EqualTo(updatedTaskValue.Value.FireDate.FireDate)
        );
    }

    [Test, Order(4)]
    public async Task DUnregisterUser()
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
