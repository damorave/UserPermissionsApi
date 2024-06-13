using Application.Common;
using ErrorOr;
using MediatR;

namespace Application.Permissions.GetAll
{
	public record GetAllPermissionsQuery() : IRequest<ErrorOr<IReadOnlyList<PermissionResponse>>>;
}
