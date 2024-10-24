namespace RocketPlaner.domain.Abstractions;

/// <summary>
/// Базовый абстрактный класс для объектов-значений домена.
/// </summary>
abstract public class DomainValueObject
{
	/// <summary>
	/// Возвращает компоненты, участвующие в проверке равенства объектов.
	/// </summary>
	/// <returns>Компоненты, участвующие в проверке равенства объектов.</returns>
	public abstract IEnumerable<object> GetEqualityComponents();

	/// <summary>
	/// Проверяет равенство текущего объекта с заданным объектом.
	/// </summary>
	/// <param name="obj">Объект для сравнения.</param>
	/// <returns>Истина, если объекты равны; ложь в противном случае.</returns>
	public override bool Equals(object? obj)
	{
		// Если объект null, то он не равен текущему объекту.
		if (obj == null)
			return false;

		// Если типы объектов не совпадают, то они не равны.
		if (GetType() != obj.GetType())
			return false;

		// Приводим объект к типу DomainValueObject.
		DomainValueObject valueObject = (DomainValueObject)obj;

		// Сравниваем компоненты равенства двух объектов.
		return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
	}

	/// <summary>
	/// Возвращает хеш-код текущего объекта.
	/// </summary>
	/// <returns>Хеш-код текущего объекта.</returns>
	public override int GetHashCode() =>
		GetEqualityComponents().Aggregate(default(int), (hashcode, value) =>
			HashCode.Combine(hashcode, value.GetHashCode()));

	/// <summary>
	/// Проверяет равенство двух объектов.
	/// </summary>
	/// <param name="left">Первый объект для сравнения.</param>
	/// <param name="right">Второй объект для сравнения.</param>
	/// <returns>Истина, если объекты равны; ложь в противном случае.</returns>
	public static bool operator ==(DomainValueObject? left, DomainValueObject? right)
	{
		// Если оба объекта null, то они равны.
		if (left is null && right is null)
			return true;

		// Если один из объектов null, а другой нет, то они не равны.
		if (left is null || right is null)
			return false;

		// Сравниваем объекты с помощью метода Equals.
		return left.Equals(right);
	}

	/// <summary>
	/// Проверяет неравенство двух объектов.
	/// </summary>
	/// <param name="left">Первый объект для сравнения.</param>
	/// <param name="right">Второй объект для сравнения.</param>
	/// <returns>Истина, если объекты не равны; ложь в противном случае.</returns>
	public static bool operator !=(DomainValueObject? left, DomainValueObject? right) =>
		!(left == right);
}
