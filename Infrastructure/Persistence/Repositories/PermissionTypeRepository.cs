using Domain.Helpers.Identify;
using Domain.PermissionTypes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
	public class PermissionTypeRepository : IPermissionTypeRepository
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public PermissionTypeRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Task<bool> ExistPermissionTypeAsync(PermissionTypeId id) => _context.PermissionType.AnyAsync(permission => permission.Id == id);

		public Task<List<PermissionType>> GetPermissions() => _context.PermissionType.ToListAsync();

		public async Task<PermissionType?> GetByIdAsync(PermissionTypeId id) => await _context.PermissionType.SingleOrDefaultAsync(c => c.Id == id);

		public void ModifyPermission(PermissionType permission) => _context.PermissionType.Update(permission);

		public void RequestPermission(PermissionType permission) => _context.PermissionType.Add(permission);
	}
}
