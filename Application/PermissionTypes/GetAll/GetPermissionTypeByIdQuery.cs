using Application.Common;
using ErrorOr;
using MediatR;

namespace Application.PermissionTypes.GetAll
{
	public record GetPermissionTypeByIdQuery(Guid Id) : IRequest<ErrorOr<PermissionTypeResponse>>;
}
