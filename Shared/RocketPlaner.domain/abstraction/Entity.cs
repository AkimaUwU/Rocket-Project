
namespace RocketPlaner.domain.abstraction;

abstract public class DomainEntity
{
    public Guid Id { get; init; }
    public DomainEntity(Guid id)
    {
        Id=id;
    }
}
