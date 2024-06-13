using Application.Common;
using Domain.Helpers.Identify;
using Domain.PermissionTypes;
using ErrorOr;
using MediatR;

namespace Application.PermissionTypes.GetAll
{
	internal sealed class GetPermissionTypeByIdQueryHandler : IRequestHandler<GetPermissionTypeByIdQuery, ErrorOr<PermissionTypeResponse>>
	{
		private readonly IPermissionTypeRepository _permissionTypeRepository;

		public GetPermissionTypeByIdQueryHandler(IPermissionTypeRepository permissionTypeRepository)
		{
			_permissionTypeRepository = permissionTypeRepository ?? throw new ArgumentNullException(nameof(permissionTypeRepository));
		}

		public async Task<ErrorOr<PermissionTypeResponse>> Handle(GetPermissionTypeByIdQuery query, CancellationToken cancellationToken)
		{
			if (await _permissionTypeRepository.GetByIdAsync(new PermissionTypeId(query.Id)) is not PermissionType customer)
			{
				return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found.");
			}

			return new PermissionTypeResponse(
				customer.Id.Value,
				customer.Descripcion.Value
				);
		}
	}
}
