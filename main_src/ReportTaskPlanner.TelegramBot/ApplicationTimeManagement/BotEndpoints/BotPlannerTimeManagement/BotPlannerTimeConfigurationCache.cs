using PRTelegramBot.Interfaces;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models;
using ReportTaskPlanner.TelegramBot.Shared.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.BotEndpoints.BotPlannerTimeManagement;

public sealed class BotPlannerTimeConfigurationCache : ITelegramCache
{
    private ApplicationTime[] _times = [];
    private List<int> _messageIds = [];

    public void AddTimes(ApplicationTime[] times) => _times = times;

    public Result<ApplicationTime> Get(string displayName)
    {
        ApplicationTime? time = _times.FirstOrDefault(t => t.DisplayName == displayName);
        return time == null ? new Error("Такого времени не существует") : time;
    }

    public void AddMessage(TGMessage? message)
    {
        if (message == null)
            return;
        _messageIds.Add(message.MessageId);
    }

    public bool ClearData()
    {
        _times = [];
        _messageIds = [];
        return true;
    }
}
