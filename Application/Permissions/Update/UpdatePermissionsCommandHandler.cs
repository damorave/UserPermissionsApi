using Domain.Helpers.Identify;
using Domain.Permissions;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Permissions.Update
{
	internal sealed class UpdatePermissionsCommandHandler : IRequestHandler<UpdatePermissionsCommand, ErrorOr<Unit>>
	{
		private readonly IPermissionRepository _permissionRepository;

		private readonly IUnitOfWork _unitOfWork;
		/// <summary>
		/// Represents a distributed cache of serialized values.
		/// </summary>
		private readonly IDistributedCache _distributedCache;


		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public UpdatePermissionsCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
		{
			_permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ErrorOr<Unit>> Handle(UpdatePermissionsCommand command, CancellationToken cancellationToken)
		{
			if (!await _permissionRepository.ExistPermissionAsync(new PermissionId(command.Id)))
			{
				return Error.NotFound("Permission.NotFound", "The permission with the provide Id was not found");
			}

			if (TextNotNull.Validate(command.NombreEmpleado) is not TextNotNull nombreEmpleado ||
			TextNotNull.Validate(command.ApellidoEmpleado) is not TextNotNull apellidoEmpleado)
			{
				return Error.Validation("Permission.nombreEmpleado o nombreEmpleado.apellidoEmpleado is not valid format");
			}

			Permission permission = Permission.UpdatePermission(
				command.Id,
				command.TipoPermiso,
				nombreEmpleado,
				apellidoEmpleado,
				command.FechaPermiso
				);

			_permissionRepository.ModifyPermission(permission);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// Se remueve la colección de redis para que cuando se vuelva a solicitar la lista será actualizada desde Get
			var cacheKey = "PermissionList";
			await _distributedCache.RemoveAsync(cacheKey);

			return Unit.Value;
		}
	}
}
