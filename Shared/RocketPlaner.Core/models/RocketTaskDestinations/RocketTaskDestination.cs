using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTaskDestinations.ValueObjects;
using RocketPlaner.Core.models.RocketTasks;

namespace RocketPlaner.Core.models.RocketTaskDestinations;

public class RocketTaskDestination : DomainEntity
{
    private RocketTaskDestination() { }

    internal RocketTaskDestination(DestinationChatId chatId, RocketTask belongsTo)
        : this()
    {
        ChatId = chatId;
        BelongsTo = belongsTo;
    }

    public RocketTask BelongsTo { get; init; } = null!;
    public DestinationChatId ChatId { get; private set; }
}
