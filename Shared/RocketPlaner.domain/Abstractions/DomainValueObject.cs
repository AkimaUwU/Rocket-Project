

namespace RocketPlaner.domain.Abstractions;

abstract public class DomainValueObject
{
	public abstract IEnumerable<object> GetEqualityComponents();

	public override bool Equals(object? obj)
	{
		if (obj == null)
			return false;

		if (GetType() != obj.GetType())
			return false;

		DomainValueObject valueObject = (DomainValueObject)obj;

		return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
	}

	public override int GetHashCode() =>
		GetEqualityComponents().Aggregate(default(int), (hashcode, value) =>
			HashCode.Combine(hashcode, value.GetHashCode()));

	public static bool operator ==(DomainValueObject? left, DomainValueObject? right)
	{
		if (left is null && right is null)
			return true;

		if (left is null || right is null)
			return false;

		return left.Equals(right);
	}

	public static bool operator !=(DomainValueObject? left, DomainValueObject? right) =>
		!(left == right);
}
