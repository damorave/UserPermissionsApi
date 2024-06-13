

namespace Domain.Primitives
{
	/// <summary>
	/// Interface encargada de manejar la transaccionalidad de los repositorios
	/// </summary>
	public interface IUnitOfWork
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
