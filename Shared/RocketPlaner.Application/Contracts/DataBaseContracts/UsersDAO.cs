namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public sealed class UsersDao
{
    public Guid Id { get; set; }
    public long TelegramId { get; set; }
    public List<TasksDao> Tasks { get; set; } = [];
}
