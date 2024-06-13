using Domain.Helpers.Identify;
using ErrorOr;
using MediatR;

namespace Application.Permissions.Create
{
	public record CreatePermissionCommand(
		PermissionTypeId TipoPermiso,
		string NombreEmpleado,
		string ApellidoEmpleado,
		DateTime FechaPermiso
		) : IRequest<ErrorOr<Unit>>;
}
