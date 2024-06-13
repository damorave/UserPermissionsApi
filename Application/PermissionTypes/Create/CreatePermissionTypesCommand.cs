using ErrorOr;
using MediatR;

namespace Application.PermissionTypes.Create
{
	public record CreatePermissionTypesCommand(
		string Descripcion) : IRequest<ErrorOr<Unit>>;
}
