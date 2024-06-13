namespace Domain.Primitives;

/// <summary>
/// Clase abstracta manejadora de eventos de dominio DDD
/// </summary>
public abstract class AggregateRoot
{
	/// <summary>
	/// Lista de dominios
	/// </summary>
	private readonly List<DomainEvents> _domainEvents = new();

	/// <summary>
	/// Encargada de recolectar los eventos de dominio que tenga el Aggregate
	/// </summary>
	/// <returns></returns>
	public ICollection<DomainEvents> GetDomainEvents() => _domainEvents;

	/// <summary>
	/// Encargado de ejecutar los eventos de dominio
	/// </summary>
	protected void Raise(DomainEvents domainEvents)
	{
		_domainEvents.Add(domainEvents);
	}
}