namespace RocketPlaner.TelegramBot.BotEndpoints.CreateTaskEndpoint;

public class CreateTaskStateContainer
{
    private int _messageId;
    private string _selectedTaskType = string.Empty;
    private string _taskTitle = string.Empty;
    private string _taskDescription = string.Empty;
    private DateTime _selectedDate = DateTime.MinValue;
    private long _userTelegramId;

    public void SetMessageId(int messageId) => _messageId = messageId;

    public void SetSelectedTaskType(string selectedTaskType) =>
        _selectedTaskType = selectedTaskType;

    public void SetTaskDescription(string taskDescription) => _taskDescription = taskDescription;

    public void SetSelectedDate(DateTime selectedDate) => _selectedDate = selectedDate;

    public void SetTaskTitle(string taskTitle) => _taskTitle = taskTitle;

    public void SetUserTelegramId(long userTelegramId) => _userTelegramId = userTelegramId;

    public void Clean()
    {
        _messageId = 0;
        _selectedTaskType = string.Empty;
        _taskDescription = string.Empty;
        _selectedDate = DateTime.MinValue;
        _taskTitle = string.Empty;
        _userTelegramId = -1;
    }

    public int MessageId => _messageId;
    public string TaskType => _selectedTaskType;
    public string TaskTitle => _taskTitle;
    public string TaskDescription => _taskDescription;
    public DateTime SelectedDate => _selectedDate;
    public long UserTelegramId => _userTelegramId;
}
