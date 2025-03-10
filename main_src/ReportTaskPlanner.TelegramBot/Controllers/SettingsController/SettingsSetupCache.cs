using PRTelegramBot.Interfaces;

namespace ReportTaskPlanner.TelegramBot.Controllers.SettingsController;

public sealed class SettingsSetupCache : ITelegramCache
{
    private readonly Dictionary<long, string> _steps = [];
    private readonly List<int> _messageIds = [];
    
    public const string start = "START_CONFIGURING";
    public const string waitsForToken = "WAITS_FOR_TOKEN";
    
    public IReadOnlyCollection<int> MessageIds => _messageIds;
    public void RegisterStep(long userId) => _steps.Add(userId, start);
    public bool HasStep(long userId) => _steps.ContainsKey(userId);
    public void UpdateStep(long userId, string step) => _steps[userId] = step;

    public void AddMessage(TGMessage message) => _messageIds.Add(message.MessageId);
    
    public bool ClearData()
    {
        _steps.Clear();
        return true;
    }
}