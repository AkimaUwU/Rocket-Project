using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;
using RocketPlaner.Core.models.Users;
using RocketPlaner.Core.Tools;

namespace RocketPlanner.Application.Tests.Samples.DomainLogic.UserTaskTests;

public class UserTaskTest
{
    [Test()]
    public void CreateValidOneLifeTask()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Съесть ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new OneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );
        Assert.That(task.IsOk, Is.EqualTo(true));
    }

    [Test()]
    public void UpdateValidDateOfNotification()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Съесть ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new OneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );

        task = task.Value.UpdateNotificationDate(DateTime.Now.AddDays(1));

        Assert.That(task.IsOk, Is.EqualTo(true));
    }

    [Test()]
    public void UpdateInvalidDateOfNotification()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Съесть ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new OneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );

        task = task.Value.UpdateNotificationDate(DateTime.Now.AddDays(-5));

        Assert.That(task.IsOk, Is.EqualTo(false));
    }

    [Test()]
    public void CreateValidNoOneLifeTask()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Съесть ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new NoOneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );
        Assert.That(task.IsOk, Is.EqualTo(true));
    }

    [Test()]
    public void CreateTaskWithoutMessageAndTitle()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "";
        RocketTaskType oneLifeType = new NoOneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );
        Assert.That(task.IsOk, Is.EqualTo(false));
    }

    [Test()]
    public void AddDestinationToATask()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Съесть ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new OneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );

        task = user.Tasks.Add(task.Value);

        task = user.Tasks.Find(t => t.Title == title);

        Result<RocketTaskDestination> destination = RocketTaskDestination.Create("321312");

        destination = task.Value.Destinations.Add(destination.Value);

        Assert.That(task.Value.Destinations.GetAll().Count, Is.EqualTo(2));
    }

    [Test()]
    public void AddDestinationDuplicate()
    {
        Result<User> userCreation = User.Create(12312323);
        User user = userCreation.Value;

        string message = "Съесть ролтон";
        string[] destinations = ["1231312"];
        DateTime notificationDate = DateTime.Now.AddDays(1);
        string title = "Ролтон";
        RocketTaskType oneLifeType = new OneLife();

        Result<RocketTask> task = RocketTask.Create(
            message,
            destinations,
            user,
            oneLifeType,
            notificationDate,
            title
        );

        task = user.Tasks.Add(task.Value);

        task = user.Tasks.Find(t => t.Title == title);

        Result<RocketTaskDestination> destination = RocketTaskDestination.Create("321312");

        destination = task.Value.Destinations.Add(destination.Value);
        destination = task.Value.Destinations.Add(destination.Value);

        Assert.That(task.Value.Destinations.GetAll().Count, Is.EqualTo(2));
    }
}
