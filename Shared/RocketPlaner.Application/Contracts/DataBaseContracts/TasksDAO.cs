namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public class TasksDao
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public UsersDao Owner { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public DateTime NotifyDate { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<DestinationsDao> Destinations { get; set; } = [];
}
