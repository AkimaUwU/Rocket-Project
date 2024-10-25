using CSharpFunctionalExtensions;
using RocketPlaner.domain.models.RocketTasks;
using RocketPlaner.domain.models.RocketTasks.ValueObjects;
using RocketPlaner.domain.models.Users;

class RocketTaskCreation 
{
 public void startwork ()
 {	
	
	// Повторяющаяся, Одноразовая	
    User user = User.Create("1234").Value;
	string message = "привет";    		    
    Result<RocketTaskType> typeRequest = RocketTaskType.Create("Одноразовая");
	if (typeRequest.IsFailure)
	{
		Console.WriteLine(typeRequest.Error);
		return;
	}

	RocketTaskType typee = typeRequest.Value;

    DateTime time = new DateTime(2024, 10, 26);
    string[] array = ["privet", "poka", "lalala"];

    Result<RocketTask> result = RocketTask.Create(message,array,user,typee,time);
	if (result.IsFailure)
	{
		Console.WriteLine(result.Error);
		return;
	}

	RocketTask createdTask = result.Value;
	Console.WriteLine($"ИД задачи: {createdTask.Id}");
	Console.WriteLine($"Дата создания задачи: {createdTask.CreatedDate}");
	Console.WriteLine($"Дата уведомления задачи: {createdTask.NotifyDate}");
	Console.WriteLine($"Тип задачи: {createdTask.Type.Type}");
	Console.WriteLine($"Пользователь: {createdTask.Owner.TelegramId}");
    foreach (var a in createdTask.Destinations)
    {
        Console.WriteLine(a.ChatId);
    }
Console.WriteLine();
    Result ok = createdTask.RemoveDestion("privet");
     foreach (var a in createdTask.Destinations)
    {
        Console.WriteLine(a.ChatId);
    }
Console.WriteLine();
    ok = createdTask.RemoveDestion("poka");
     foreach (var a in createdTask.Destinations)
    {
        Console.WriteLine(a.ChatId);
    }
Console.WriteLine();
    ok = createdTask.RemoveDestion("lalala");
    if (ok.IsFailure) Console.WriteLine(ok.Error);
     foreach (var a in createdTask.Destinations)
    {
        Console.WriteLine(a.ChatId);
    }
 }
}