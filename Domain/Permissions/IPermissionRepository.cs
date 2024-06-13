using Domain.Helpers.Identify;

namespace Domain.Permissions
{
	/// <summary>
	/// Permite una abstracción al patrón repositorio logrando independencia de la fuente de datos
	/// </summary>
	public interface IPermissionRepository
	{
		/// <summary>
		/// Creación de un permiso
		/// </summary>
		/// <param name="permission">Objeto con la información del permiso</param>
		void RequestPermission(Permission permission);
		/// <summary>
		/// Método encargado de actualizar un permiso
		/// </summary>
		void ModifyPermission(Permission permission);
		/// <summary>
		/// Firma definida para obtener lista de permisos
		/// </summary>
		/// <param name="id">id del permiso</param>
		/// <returns>Retorna lista de permisos/returns>
		Task<List<Permission>> GetPermissions();
		/// <summary>
		/// Obtiene un permiso por ID
		/// </summary>
		Task<Permission?> GetByIdAsync(PermissionId id);
		/// <summary>
		/// Firma que valida si existe el permiso a modificar
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> ExistPermissionAsync(PermissionId id);

	}
}
