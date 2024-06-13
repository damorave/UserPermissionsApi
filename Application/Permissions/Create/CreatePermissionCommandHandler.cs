using Domain.Helpers.Identify;
using Domain.Permissions;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Permissions.Create
{
	internal sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ErrorOr<Unit>>
	{
		/// <summary>
		/// Permite una abstracción al patrón repositorio logrando independencia de la fuente de datos
		/// </summary>
		private readonly IPermissionRepository _permissionRepository;
		/// <summary>
		/// Interface encargada de manejar la transaccionalidad de los repositorios
		/// </summary>
		private readonly IUnitOfWork _unitOfWork;
		/// <summary>
		/// Represents a distributed cache of serialized values.
		/// </summary>
		private readonly IDistributedCache _distributedCache;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IDistributedCache distributedCache)
		{
			_permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		public async Task<ErrorOr<Unit>> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
		{
			if (TextNotNull.Validate(command.NombreEmpleado) is not TextNotNull nombreEmpleado ||
			TextNotNull.Validate(command.ApellidoEmpleado) is not TextNotNull apellidoEmpleado)
			{
				return Error.Validation("Permission.nombreEmpleado o nombreEmpleado.apellidoEmpleado is not valid format");
			}

			var permission = new Permission(
				new PermissionId(Guid.NewGuid()),
				command.TipoPermiso,
				nombreEmpleado,
				apellidoEmpleado,
				command.FechaPermiso
				);

			_permissionRepository.RequestPermission(permission);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// Se remueve la colección de redis para que cuando se vuelva a solicitar la lista será actualizada desde Get
			var cacheKey = "PermissionList";
			await _distributedCache.RemoveAsync(cacheKey);

			return Unit.Value;
		}

	}
}
