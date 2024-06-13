using ErrorOr;
using MediatR;

namespace Application.Permissions.Update
{
	public record UpdatePermissionsCommand(
		Guid Id,
		Guid TipoPermiso,
		string NombreEmpleado,
		string ApellidoEmpleado,
		DateTime FechaPermiso
		) : IRequest<ErrorOr<Unit>>;
}
