

using System.Data;
using CSharpFunctionalExtensions;
using RocketPlaner.domain.abstraction;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;

namespace RocketPlaner.domain.models.RocketTasks;

public class RocketTask : DomainEntity
{
    private RocketTask() : base(Guid.Empty)
    {
        TextTask = string.Empty;
        TypeTask = RocketTaskType.Defoult;
        NotificationTimeTask= DateTime.MinValue;
        CreateTimeTask = DateTime.MinValue;
    }
    private RocketTask(string text, RocketTaskType type, DateTime ApdateTimeTask) : base(Guid.NewGuid())
    {
        TextTask = text;
        TypeTask = type;
        NotificationTimeTask= ApdateTimeTask;
        CreateTimeTask = DateTime.Now;
    }
    public DateTime CreateTimeTask {get; init;}
    public DateTime NotificationTimeTask{get; private set;}
    public string TextTask{get; private set;}
    public RocketTaskType TypeTask {get; private set;}

    public static Result <RocketTask> Create(string text, RocketTaskType type, DateTime ApdateTimeTask) 
    {
        return new RocketTask(text, type, ApdateTimeTask);
    }
}
