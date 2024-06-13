using Application.Common;
using ErrorOr;
using MediatR;

namespace Application.PermissionTypes.GetAll
{
	public record GetAllPermissionTypesQuery() : IRequest<ErrorOr<IReadOnlyList<PermissionTypeResponse>>>;
}
