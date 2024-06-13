

using Domain.Permissions;
using Domain.PermissionTypes;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
	public interface IApplicationDbContext
	{
		/// <summary>
		/// DbSet get and set of Products
		/// </summary>
		public DbSet<Permission> Permission { get; set; }
		/// <summary>
		/// DbSet get and set of Products
		/// </summary>
		public DbSet<PermissionType> PermissionType { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
