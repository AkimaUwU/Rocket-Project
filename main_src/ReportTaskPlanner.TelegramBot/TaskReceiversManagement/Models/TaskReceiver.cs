namespace ReportTaskPlanner.TelegramBot.TaskReceiversManagement.Models;

public sealed class TaskReceiver
{
    public long Id { get; private set; }
    public bool IsEnabled { get; private set; }

    private TaskReceiver() { } // lite db constructor

    public TaskReceiver(long id)
    {
        Id = id;
        IsEnabled = false;
    }

    public void EnableReceiver() => IsEnabled = true;

    public void DisableReceiver() => IsEnabled = false;
}
