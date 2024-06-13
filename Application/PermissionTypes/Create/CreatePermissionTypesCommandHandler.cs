using Domain.Helpers.Identify;
using Domain.PermissionTypes;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.PermissionTypes.Create
{
	internal sealed class CreatePermissionTypesCommandHandler : IRequestHandler<CreatePermissionTypesCommand, ErrorOr<Unit>>
	{
		/// <summary>
		/// Permite una abstracción al patrón repositorio logrando independencia de la fuente de datos
		/// </summary>
		private readonly IPermissionTypeRepository _permissionTypeRepository;
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
		public CreatePermissionTypesCommandHandler(IPermissionTypeRepository permissionTypeRepository, IUnitOfWork unitOfWork, IDistributedCache distributedCache)
		{
			_permissionTypeRepository = permissionTypeRepository ?? throw new ArgumentNullException(nameof(permissionTypeRepository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache)); ;
		}

		public async Task<ErrorOr<Unit>> Handle(CreatePermissionTypesCommand command, CancellationToken cancellationToken)
		{

			if (TextNotNull.Validate(command.Descripcion) is not TextNotNull description)
			{
				throw new ArgumentException(nameof(description));
			}

			var permissionType = new PermissionType(
				new PermissionTypeId(Guid.NewGuid()),
				description
				);
			_permissionTypeRepository.RequestPermission(permissionType);

			await _unitOfWork.SaveChangesAsync();

			// Se remueve la colección de redis para que cuando se vuelva a solicitar la lista será actualizada desde Get
			var cacheKey = "PermissionTypeList";
			await _distributedCache.RemoveAsync(cacheKey);

			return Unit.Value;
		}
	}
}
