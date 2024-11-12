namespace RocketPlaner.Application.Contracts.DataBaseContracts;

public class DestinationsDao
{
    public Guid Id { get; set; }
    public string ChatId { get; set; } = string.Empty;
    public TasksDao Task { get; set; } = null!;
    public Guid TaskId { get; set; }
}
