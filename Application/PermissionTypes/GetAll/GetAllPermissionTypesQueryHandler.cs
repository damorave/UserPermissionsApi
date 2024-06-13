using Application.Common;
using Domain.PermissionTypes;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.PermissionTypes.GetAll
{
	internal sealed class GetAllPermissionTypesQueryHandler : IRequestHandler<GetAllPermissionTypesQuery, ErrorOr<IReadOnlyList<PermissionTypeResponse>>>
	{
		private readonly IPermissionTypeRepository _permissionTypeRepository;
		/// <summary>
		/// Represents a distributed cache of serialized values.
		/// </summary>
		private readonly IDistributedCache _distributedCache;


		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public GetAllPermissionTypesQueryHandler(IPermissionTypeRepository permissionTypeRepository, IDistributedCache distributedCache)
		{
			_permissionTypeRepository = permissionTypeRepository ?? throw new ArgumentNullException(nameof(permissionTypeRepository));
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		public async Task<ErrorOr<IReadOnlyList<PermissionTypeResponse>>> Handle(GetAllPermissionTypesQuery request, CancellationToken cancellationToken)
		{
			string cacheKey = "PermissionTypeList";

			IReadOnlyList<PermissionType> permissionType = await _permissionTypeRepository.GetPermissions();

			var result = permissionType.Select(permission => new PermissionTypeResponse(
				permission.Id.Value,
				permission.Descripcion.Value
			)).ToList();

			// Implementacion de RedisCache para persistencia de datos
			var redisListPermission = await _distributedCache.GetStringAsync(cacheKey);

			List<PermissionType>? permissionList;

			if (redisListPermission != null)
			{
				permissionList = JsonConvert.DeserializeObject<List<PermissionType>>(redisListPermission);
			}
			else
			{
				permissionList = permissionType as List<PermissionType>;
				var serializedPermissionList = JsonConvert.SerializeObject(permissionList);
				await _distributedCache.SetStringAsync(cacheKey, serializedPermissionList);
			}

			return result;
		}
	}
}
