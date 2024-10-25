using RocketPlaner.domain.Abstractions;

public abstract class DomainAggregateRoot : DomainEntity
{
    protected DomainAggregateRoot(Guid id) : base(id)
    {
    }
}