using ErrorOr;
using MediatR;

namespace Application.PermissionTypes.Update
{
	public record UpdatePermissionsTypeCommand(
		Guid Id,
		string Descripcion
		) : IRequest<ErrorOr<Unit>>;
}
