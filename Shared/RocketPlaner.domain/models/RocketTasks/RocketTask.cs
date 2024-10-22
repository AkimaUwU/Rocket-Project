using CSharpFunctionalExtensions;

using RocketPlaner.domain.Abstractions;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;

namespace RocketPlaner.domain.models.RocketTasks;

public class RocketTask : DomainEntity
{
	public DateTime CreatedTime { get; init; }
	public DateTime NotifyTime { get; private set; }
	public string Message { get; private set; }
	public RocketTaskType Type { get; private set; }

	private RocketTask() : base(Guid.Empty)
	{
		Message = string.Empty;
		Type = RocketTaskType.Defoult;
		NotifyTime = DateTime.MinValue;
		CreatedTime = DateTime.MinValue;
	}

	private RocketTask(string text, RocketTaskType type, DateTime ApdateTimeTask) : base(Guid.NewGuid())
	{
		Message = text;
		Type = type;
		NotifyTime = ApdateTimeTask;
		CreatedTime = DateTime.Now;
	}

	public static Result<RocketTask> Create(string text, RocketTaskType type, DateTime ApdateTimeTask)
	{
		return new RocketTask(text, type, ApdateTimeTask);
	}
}
