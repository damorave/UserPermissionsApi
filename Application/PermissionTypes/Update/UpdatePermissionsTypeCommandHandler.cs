using Domain.Helpers.Identify;
using Domain.PermissionTypes;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.PermissionTypes.Update
{
	internal sealed class UpdatePermissionsTypeCommandHandler : IRequestHandler<UpdatePermissionsTypeCommand, ErrorOr<Unit>>
	{
		private readonly IPermissionTypeRepository _permissionTypeRepository;

		private readonly IUnitOfWork _unitOfWork;
		/// <summary>
		/// Represents a distributed cache of serialized values.
		/// </summary>
		private readonly IDistributedCache _distributedCache;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public UpdatePermissionsTypeCommandHandler(IPermissionTypeRepository permissionTypeRepository, IUnitOfWork unitOfWork = null, IDistributedCache distributedCache = null)
		{
			_permissionTypeRepository = permissionTypeRepository ?? throw new ArgumentNullException(nameof(permissionTypeRepository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		public async Task<ErrorOr<Unit>> Handle(UpdatePermissionsTypeCommand command, CancellationToken cancellationToken)
		{
			if (!await _permissionTypeRepository.ExistPermissionTypeAsync(new PermissionTypeId(command.Id)))
			{
				return Error.NotFound("PermissionType.NotFound", "The permission type with the provide Id was not found");
			}

			if (TextNotNull.Validate(command.Descripcion) is not TextNotNull descripcion)
			{
				return Error.Validation("Permission.descripcion is not valid format");
			}

			PermissionType permissionType = PermissionType.UpdatePermissionType(
				command.Id,
				descripcion
				);

			_permissionTypeRepository.ModifyPermission(permissionType);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// Se remueve la colección de redis para que cuando se vuelva a solicitar la lista será actualizada desde Get
			var cacheKey = "PermissionTypeList";
			await _distributedCache.RemoveAsync(cacheKey);

			return Unit.Value;

		}
	}
}
