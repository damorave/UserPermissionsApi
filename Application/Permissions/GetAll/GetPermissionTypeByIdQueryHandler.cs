using Application.Common;
using Domain.Helpers.Identify;
using Domain.Permissions;
using ErrorOr;
using MediatR;

namespace Application.Permissions.GetAll
{
	internal sealed class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, ErrorOr<PermissionResponse>>
	{
		private readonly IPermissionRepository _permissionRepository;

		public GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
		{
			_permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
		}

		public async Task<ErrorOr<PermissionResponse>> Handle(GetPermissionByIdQuery query, CancellationToken cancellationToken)
		{
			if (await _permissionRepository.GetByIdAsync(new PermissionId(query.Id)) is not Permission customer)
			{
				return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found.");
			}

			return new PermissionResponse(
				customer.Id.Value,
				customer.TipoPermiso.Value,
				customer.NombreEmpleado.Value,
				customer.ApellidoEmpleado.Value,
				customer.FechaPermiso
				);
		}
	}
}
