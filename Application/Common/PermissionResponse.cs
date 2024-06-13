namespace Application.Common
{
	public record PermissionResponse(
		Guid Id,
		Guid TipoPermiso,
		string NombreEmpleado,
		string ApellidoEmpleado,
		DateTime FechaPermiso);

	public record PermissionTypeResponse(
		Guid Id,
		string Descripcion);
}
