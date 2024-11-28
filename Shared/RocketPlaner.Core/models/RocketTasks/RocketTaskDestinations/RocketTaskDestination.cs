using RocketPlaner.Core.Abstractions;
using RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations.ValueObjects;

namespace RocketPlaner.Core.models.RocketTasks.RocketTaskDestinations;

public class RocketTaskDestination : DomainEntity
{
    private RocketTaskDestination()
        : base(Guid.Empty) { }

    internal RocketTaskDestination(
        DestinationChatId chatId,
        RocketTask belongsTo,
        Guid id = default
    )
        : base(id == default ? Guid.NewGuid() : id)
    {
        ChatId = chatId;
        BelongsTo = belongsTo;
    }

    public RocketTask BelongsTo { get; init; }
    public DestinationChatId ChatId { get; private set; }
}
