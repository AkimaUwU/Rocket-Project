using RocketPlaner.Application.Contracts.DataBaseContracts;
using RocketPlaner.Application.Users.Commands.AddTaskForUsers;
using RocketPlaner.Application.Users.Commands.RegisterUser;
using RocketPlaner.Application.Users.Commands.RemoveTaskForUsers;
using RocketPlaner.Application.Users.Commands.UpdateTaskDate;
using RocketPlaner.DataAccess.DatabaseImplementations.TasksDatabase;
using RocketPlaner.DataAccess.DatabaseImplementations.UsersDatabase;

namespace RocketPlaner.Tests.TasksTests;

public class TaskTest
{
    [Test]
    public async Task TestCreateValidTask()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();

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
        Assert.That(addedTask, Is.Not.Null);
    }

    [Test]
    public async Task TestRemoveTask()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();

        long telegramId = 123;
        var command = new RegisterUserCommand(123);
        var handler = new RegisterUserCommandHandler(usersDb);
        var registeredUser = await handler.Handle(command);

        var title = "Ролтон";

        var removeTaskCommand = new RemoveTaskForUsersCommand(telegramId, title);
        var removeTaskCommandHandler = new RemoveTaskForUserCommandHandler(tasksDb, usersDb);
        var removedTask = await removeTaskCommandHandler.Handle(removeTaskCommand);
        Assert.That(removedTask, Is.Not.Null);
    }

    [Test]
    public async Task TestUpdateTask()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();

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

        var updatedDate = DateTime.Now.AddHours(2);
        var updateTaskCommand = new UpdateTaskDateCommand(telegramId, title, updatedDate);
        var updateTaskCommandHandler = new UpdateTaskDateCommandHandler(tasksDb, usersDb);
        addedTask = await updateTaskCommandHandler.Handle(updateTaskCommand);

        Assert.That(addedTask.Value.NotifyDate, Is.EqualTo(updatedDate));
    }

    [Test]
    public async Task AddDuplicateTask()
    {
        IUsersDataBase usersDb = new UsersDatabase();
        ITaskDataBase tasksDb = new TasksDatabase();

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
        addedTask = await addTaskHandler.Handle(addTaskCommand);
        Assert.That(addedTask.IsError, Is.True);
    }
}
