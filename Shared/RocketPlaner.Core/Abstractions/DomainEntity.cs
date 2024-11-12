namespace RocketPlaner.Core.Abstractions;

// Абстрактный класс - сущность
public abstract class DomainEntity
{
    // Идентификатор сущности
    public Guid Id { get; init; }

    // Конструктор создания сущности
    public DomainEntity(Guid id) => Id = id;
}
