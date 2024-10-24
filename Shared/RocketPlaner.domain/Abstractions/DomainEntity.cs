
namespace RocketPlaner.domain.Abstractions;

// Абстрактный класс - сущность
abstract public class DomainEntity
{
	// Идентификатор сущности
	public Guid Id { get; init; }

	// Конструктор создания сущности
	public DomainEntity(Guid id)
	{
		Id = id;
	}
}
