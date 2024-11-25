using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

namespace RocketPlaner.Tests.UserTests;

public class UserCreateTest
{
    [Test]
    public async Task CreateAndDeleteUser_ValidUser()
    {
        long telegramId = 123;
        IUsersDataBase usersDb = new UsersDatabase();
        RegisterUserCommand command = new RegisterUserCommand(123);
        RegisterUserCommandHandler handler = new RegisterUserCommandHandler(usersDb);
        await handler.Handle(command);
        var user = await usersDb.GetUser(123);
        Assert.That(user, Is.Not.Null);
        await usersDb.RemoveUser(user);
    }

    [Test]
    public async Task CreateUser_Invalid()
    {
        long telegramId = 0;
        IUsersDataBase usersDb = new UsersDatabase();
        RegisterUserCommand command = new RegisterUserCommand(telegramId);
        RegisterUserCommandHandler handler = new RegisterUserCommandHandler(usersDb);
        var registeredUser = await handler.Handle(command);
        Assert.That(registeredUser.IsError, Is.True);
    }

    [Test]
    public async Task Get_User()
    {
        long telegramId = 123;
        IUsersDataBase usersDb = new UsersDatabase();
        RegisterUserCommand command = new RegisterUserCommand(telegramId);
        RegisterUserCommandHandler handler = new RegisterUserCommandHandler(usersDb);
        await handler.Handle(command);
        var user = await usersDb.GetUser(123);
        Assert.That(user, Is.Not.Null);
        await usersDb.RemoveUser(user);
    }

    [Test]
    public async Task Register_DuplicateUsers()
    {
        long telegramId = 123;

        IUsersDataBase usersDb = new UsersDatabase();
        RegisterUserCommand command = new RegisterUserCommand(telegramId);
        RegisterUserCommandHandler handler = new RegisterUserCommandHandler(usersDb);
        var registeredUser = await handler.Handle(command);

        var userCopyFromDb = await usersDb.GetUser(telegramId);

        registeredUser = await handler.Handle(command);

        Assert.That(registeredUser.IsError, Is.True);
        await usersDb.RemoveUser(userCopyFromDb!);
    }
}
