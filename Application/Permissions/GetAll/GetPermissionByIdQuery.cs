using Application.Common;
using ErrorOr;
using MediatR;

namespace Application.Permissions.GetAll
{
	public record GetPermissionByIdQuery(Guid Id) : IRequest<ErrorOr<PermissionResponse>>;
}
