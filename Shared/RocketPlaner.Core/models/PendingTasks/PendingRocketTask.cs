using RocketPlaner.Core.models.RocketTasks;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;
using RocketPlaner.Core.models.RocketTasks.ValueObjects;

namespace RocketPlaner.Core.models.PendingTasks;

public sealed class PendingRocketTask
{
    public Guid Id { get; init; }
    public long SenderId { get; init; }
    public string Title { get; init; }
    public string Text { get; init; }
    public string Type { get; init; }
    public DateTime FireDate { get; init; }
    public bool IsPeriodic { get; init; }

    private readonly List<PendingRocketTaskChat> _chatIds;

    public IReadOnlyList<PendingRocketTaskChat> ChatIds => _chatIds.ToList();

    internal PendingRocketTask(RocketTask task)
    {
        Id = task.Id;
        SenderId = task.Owner.TelegramId.TelegramId;
        Title = task.Title.Title;
        Text = task.Message.Message;
        Type = task.Type.Type;
        FireDate = task.FireDate.FireDate;
        if (Type == RocketTaskType.Periodic)
            IsPeriodic = true;
        _chatIds = InitializeChatIds(task);
    }

    private static List<PendingRocketTaskChat> InitializeChatIds(RocketTask task)
    {
        List<PendingRocketTaskChat> chatIds = [];
        chatIds.AddRange(task.Destinations.Select(chat => new PendingRocketTaskChat(chat)));
        return chatIds;
    }
}

public sealed class PendingRocketTaskChat
{
    public Guid Id { get; init; }
    public long ChatId { get; init; }

    internal PendingRocketTaskChat(RocketTaskDestination destination)
    {
        Id = destination.Id;
        ChatId = destination.ChatId.ChatId;
    }
}
