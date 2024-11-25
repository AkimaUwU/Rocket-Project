using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.RocketTasks.Commands.AddDestinationForRocketTask;
using RocketPlaner.Application.RocketTasks.Commands.RemoveDestinationFromRocketTask;
using RocketPlaner.Application.Users.Commands.AddTaskForUsers;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.DataAccess.DatabaseImplementations.TasksDatabase;
using RocketPlaner.DataAccess.DatabaseImplementations.TasksDestinationsDataBase;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

namespace RocketPlaner.Tests.DestinationsTests;

public class DestinationTest
{
    [Test]
    public async Task CreateDestinationTest()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();
        ITaskDestinationDatabase destDb = new TaskDestinationsDatabase();

        long telegramId = 123;
        var command = new RegisterUserCommand(123);
        var handler = new RegisterUserCommandHandler(usersDb);
        var registeredUser = await handler.Handle(command);

        var message = "Заварить ролтон";
        var typeTask = "Одноразовая";
        var notifyDate = DateTime.Now.AddHours(1);
        var title = "Ролтон";

        var addTaskCommand = new AddTaskForUsersCommand(
            message,
            telegramId,
            typeTask,
            notifyDate,
            title
        );
        var addTaskHandler = new AddTaskForUsersCommandHandler(usersDb, tasksDb);
        var addedTask = await addTaskHandler.Handle(addTaskCommand);

        string chatId = "123";

        var addDestination = new AddDestinationForRocketTaskCommand(telegramId, title, chatId);
        var addDestinationHandler = new AddDestinationForRocketTaskCommandHandler(usersDb, destDb);
        var addedDestionation = await addDestinationHandler.Handle(addDestination);

        Assert.That(addedDestionation.Value.ChatId, Is.EqualTo(chatId));
    }

    [Test]
    public async Task RemoveDestinationTest()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();
        ITaskDestinationDatabase destDb = new TaskDestinationsDatabase();

        long telegramId = 123;
        var command = new RegisterUserCommand(123);
        var handler = new RegisterUserCommandHandler(usersDb);
        var registeredUser = await handler.Handle(command);

        var message = "Заварить ролтон";
        var typeTask = "Одноразовая";
        var notifyDate = DateTime.Now.AddHours(1);
        var title = "Ролтон";

        var addTaskCommand = new AddTaskForUsersCommand(
            message,
            telegramId,
            typeTask,
            notifyDate,
            title
        );
        var addTaskHandler = new AddTaskForUsersCommandHandler(usersDb, tasksDb);
        var addedTask = await addTaskHandler.Handle(addTaskCommand);

        string chatId = "123";

        var addDestination = new AddDestinationForRocketTaskCommand(telegramId, title, chatId);
        var addDestinationHandler = new AddDestinationForRocketTaskCommandHandler(usersDb, destDb);
        var addedDestionation = await addDestinationHandler.Handle(addDestination);

        var removeDestination = new RemoveDestinationFromRocketTaskCommand(
            telegramId,
            title,
            chatId
        );
        var removeDestinationHandler = new RemoveDestinationFromRocketTaskCommandHandler(
            usersDb,
            destDb
        );
        var removedDestination = await removeDestinationHandler.Handle(removeDestination);

        Assert.That(removedDestination.Value.ChatId, Is.EqualTo(addedDestionation.Value.ChatId));
    }

    [Test]
    public async Task AddDuplicateDestination()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();
        ITaskDestinationDatabase destDb = new TaskDestinationsDatabase();

        long telegramId = 123;
        var command = new RegisterUserCommand(123);
        var handler = new RegisterUserCommandHandler(usersDb);
        var registeredUser = await handler.Handle(command);

        var message = "Заварить ролтон";
        var typeTask = "Одноразовая";
        var notifyDate = DateTime.Now.AddHours(1);
        var title = "Ролтон";

        var addTaskCommand = new AddTaskForUsersCommand(
            message,
            telegramId,
            typeTask,
            notifyDate,
            title
        );
        var addTaskHandler = new AddTaskForUsersCommandHandler(usersDb, tasksDb);
        var addedTask = await addTaskHandler.Handle(addTaskCommand);

        string chatId = "123";

        var addDestination = new AddDestinationForRocketTaskCommand(telegramId, title, chatId);
        var addDestinationHandler = new AddDestinationForRocketTaskCommandHandler(usersDb, destDb);
        var addedDestionation = await addDestinationHandler.Handle(addDestination);
        addedDestionation = await addDestinationHandler.Handle(addDestination);

        Assert.That(addedDestionation.IsError, Is.True);
    }
}
