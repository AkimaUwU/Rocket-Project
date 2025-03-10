using PRTelegramBot.Interfaces;
using ReportTaskPlanner.Main.TimeManagement;
using ReportTaskPlanner.Utilities.ResultPattern;

namespace ReportTaskPlanner.TelegramBot.Controllers.BotPlannerTimeManagement;

public sealed class BotPlannerTimeConfigurationCache : ITelegramCache
{
    private readonly List<PlannerTime> _plannerTimes = [];
    private readonly List<int> _messageIds = [];
    public IReadOnlyCollection<int> MessageIds => _messageIds;
    public IReadOnlyCollection<PlannerTime> Times => _plannerTimes;

    public void AddTimes(IEnumerable<PlannerTime> plannerTimes) =>
        _plannerTimes.AddRange(plannerTimes);

    public Result<PlannerTime> Get(string displayName)
    {
        PlannerTime? time = _plannerTimes.FirstOrDefault(t => t.DisplayName == displayName);
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
        _plannerTimes.Clear();
        return true;
    }
}
