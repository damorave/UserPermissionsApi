using Domain.Helpers.Identify;
using Domain.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
	public class PermissionRepository : IPermissionRepository
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public PermissionRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<bool> ExistPermissionAsync(PermissionId id) => await _context.Permission.AnyAsync(permission => permission.Id == id);

		public async Task<List<Permission>> GetPermissions() => await _context.Permission.ToListAsync();
		public async Task<Permission?> GetByIdAsync(PermissionId id) => await _context.Permission.SingleOrDefaultAsync(c => c.Id == id);

		public void ModifyPermission(Permission permission) => _context.Permission.Update(permission);

		public void RequestPermission(Permission permission) => _context.Permission.Add(permission);
	}
}
