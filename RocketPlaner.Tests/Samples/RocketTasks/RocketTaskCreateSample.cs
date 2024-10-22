using CSharpFunctionalExtensions;
using RocketPlaner.domain.models.RocketTasks;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;
using RocketPlaner.Tests.Models;

namespace RocketPlaner.Tests.Samples.RocketTasks;

public class RocketTaskCreateSample : TestSample<RocketTask>
{
	public override Result<RocketTask> Invoke()
	{
		string text = "test text";
		DateTime notificationDate = DateTime.Now;
		Result<RocketTaskType> type = RocketTaskType.Create("Одноразовая");
		Result<RocketTask> task1 = RocketTask.Create(text, type.Value, notificationDate);

		if (task1.IsSuccess)
		{
			Console.WriteLine($"Создана задача: {task1.Value.Message}");
			Console.WriteLine($"Тип задачи: {task1.Value.Type.Type}");
			Console.WriteLine($"Дата создания: {task1.Value.CreatedTime}");
			Console.WriteLine($"Дата уведомления: {task1.Value.NotifyTime}");
		}

		if (task1.IsFailure)
		{
			Console.WriteLine(task1.Error);
		}

		Result<RocketTaskType> type2 = RocketTaskType.Create("Повторяющаяся");
		Result<RocketTask> task2 = RocketTask.Create(text, type2.Value, notificationDate);

		if (task1.IsSuccess)
		{
			Console.WriteLine($"Создана задача: {task2.Value.Message}");
			Console.WriteLine($"Тип задачи: {task2.Value.Type.Type}");
			Console.WriteLine($"Дата создания: {task2.Value.CreatedTime}");
			Console.WriteLine($"Дата уведомления: {task2.Value.NotifyTime}");
		}

		if (task2.IsFailure)
		{
			Console.WriteLine(task2.Error);
		}

		Result<RocketTaskType> type3 = RocketTaskType.Create("Другое название");
		if (type3.IsFailure)
		{
			Console.WriteLine(type3.Error);
		}

		return task1;
	}
}
