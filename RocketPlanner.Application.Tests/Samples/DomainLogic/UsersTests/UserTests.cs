using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlanner.Application.Tests.Samples.DomainLogic.UsersTests;

public class UserTests
{
    [Test()]
    public void CreateValidUser()
    {
        Result<User> user = User.Create(12312321313);
        Assert.That(user.IsOk, Is.EqualTo(true));
    }

    [Test()]
    public void CreateInvalidUser()
    {
        Result<User> user = User.Create(-312321321);
        Assert.That(user.IsOk, Is.EqualTo(false));
    }

    [Test()]
    public void CreateUserWithDefaultLong()
    {
        Result<User> user = User.Create(default);
        Assert.That(user.IsOk, Is.EqualTo(false));
    }

    [Test()]
    public void AddTaskForUser()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Купить ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new NoOneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            "Повторяющаяся",
            notificationDate,
            title
        );

        task = user.Tasks.Add(task.Value);
        Assert.That(user.Tasks.GetAll().Count, Is.EqualTo(1));
    }

    [Test()]
    public void AddDuplicateTaskForUser()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Купить ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new NoOneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            "Повторяющаяся",
            notificationDate,
            title
        );

        task = user.Tasks.Add(task.Value);
        task = user.Tasks.Add(task.Value);
        Assert.That(user.Tasks.GetAll().Count, Is.EqualTo(1));
    }

    [Test()]
    public void FindTasksOfUser()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Купить ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new NoOneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            "Повторяющаяся",
            notificationDate,
            title
        );

        task = user.Tasks.Add(task.Value);
        task = user.Tasks.Find(t => t.Title == title);
        Assert.That(task.Value.Message, Is.EqualTo(message));
    }

    [Test()]
    public void RemoveTasksOfUser()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Купить ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new NoOneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            "Повторяющаяся",
            notificationDate,
            title
        );

        task = user.Tasks.Add(task.Value);
        task = user.Tasks.Find(t => t.Title == title);
        task = user.Tasks.Remove(task.Value);
        Assert.That(task.Value.Message, Is.EqualTo(message));
    }
}
