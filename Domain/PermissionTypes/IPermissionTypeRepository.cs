
using Domain.Helpers.Identify;

namespace Domain.PermissionTypes
{
	public interface IPermissionTypeRepository
	{
		/// <summary>
		/// Creación de un tipo permiso
		/// </summary>
		/// <param name="permission">Objeto con la información del tipo permiso</param>
		void RequestPermission(PermissionType permission);
		/// <summary>
		/// Método encargado de actualizar un tipo de permiso
		/// </summary>
		void ModifyPermission(PermissionType permission);
		/// <summary>
		/// Firma definida para obtener lista de tipos de permisos
		/// </summary>
		/// <param name="id">id del tipo de permiso</param>
		/// <returns>Retorna lista de tipos de permisos/returns>
		Task<List<PermissionType>> GetPermissions();
		/// <summary>
		/// Obtiene un tipo de permiso por ID
		/// </summary>
		Task<PermissionType?> GetByIdAsync(PermissionTypeId id);
		/// <summary>
		/// Firma que valida si existe el permiso a modificar
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> ExistPermissionTypeAsync(PermissionTypeId id);
	}
}
