
namespace RocketPlaner.domain.Abstractions;

abstract public class DomainEntity
{
	public Guid Id { get; init; }

	public DomainEntity(Guid id)
	{
		Id = id;
	}
}
