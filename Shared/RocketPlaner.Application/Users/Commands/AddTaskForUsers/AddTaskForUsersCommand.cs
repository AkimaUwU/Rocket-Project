using RocketPlaner.Application.Contracts.Operations;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Application.Users.Commands.AddTaskForUsers;

public class AddTaskForUsersCommand(
    string message,
    long idUser,
    string typeTask,
    DateTime notifyDate,
    string title
) : ICommand<RocketTask>
{
    public string Message { get; init; } = message;

    public long IdUser { get; init; } = idUser;

    public string TypeTask { get; init; } = typeTask;

    public DateTime NotifyDate { get; init; } = notifyDate;

    public string TitleTask { get; init; } = title;
}
