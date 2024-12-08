namespace RocketPlaner.Core.Abstractions;

public abstract class DomainEntity
{
    protected List<IDomainEvent> Events = [];

    public void RaiseEvent(IDomainEvent domainEvent) => Events.Add(domainEvent);

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => Events.ToList();

    public void ClearEvents() => Events.Clear();
}
