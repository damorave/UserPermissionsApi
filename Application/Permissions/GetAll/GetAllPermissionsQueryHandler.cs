using Application.Common;
using Domain.Permissions;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Permissions.GetAll
{
	internal sealed class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, ErrorOr<IReadOnlyList<PermissionResponse>>>
	{
		private readonly IPermissionRepository _permissionRepository;
		private readonly IDistributedCache _distributedCache;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository, IDistributedCache distributedCache)
		{
			_permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		public async Task<ErrorOr<IReadOnlyList<PermissionResponse>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
		{
			string cacheKey = "PermissionList";

			IReadOnlyList<Permission> permission = await _permissionRepository.GetPermissions();

			var result = permission.Select(permission => new PermissionResponse(
				permission.Id.Value,
				permission.TipoPermiso.Value,
				permission.NombreEmpleado.Value,
				permission.ApellidoEmpleado.Value,
				permission.FechaPermiso
				)).ToList();

			// Implementacion de RedisCache para persistencia de datos
			var redisListPermission = await _distributedCache.GetStringAsync(cacheKey);

			List<Permission>? permissionList;

			if (redisListPermission != null)
			{
				permissionList = JsonConvert.DeserializeObject<List<Permission>>(redisListPermission);
			}
			else
			{
				permissionList = permission as List<Permission>;
				var serializedPermissionList = JsonConvert.SerializeObject(permissionList);
				await _distributedCache.SetStringAsync(cacheKey, serializedPermissionList);
			}

			return result;
		}
	}
}
