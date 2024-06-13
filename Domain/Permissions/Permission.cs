using Domain.Helpers.Identify;
using Domain.PermissionTypes;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Permissions
{
	/// <summary>
	/// Clase que contiene las propiedades para permisos
	/// </summary>
	public sealed class Permission : AggregateRoot
	{
		/// <summary>
		/// Crea una instancia de la clase, seteo de propiedades
		/// </summary>
		public Permission(PermissionId id, PermissionTypeId tipoPermiso, TextNotNull nombreEmpleado, TextNotNull apellidoEmpleado, DateTime fechaPermiso)
		{
			Id = id;
			TipoPermiso = tipoPermiso;
			NombreEmpleado = nombreEmpleado;
			ApellidoEmpleado = apellidoEmpleado;
			FechaPermiso = fechaPermiso;
		}

		public Permission()
		{

		}

		/// <summary>
		/// Id del permiso
		/// </summary>
		public PermissionId Id { get; private set; }
		/// <summary>
		/// Id del tipo de permiso
		/// </summary>
		public PermissionTypeId TipoPermiso { get; private set; }
		/// <summary>
		/// Nombre del empleado
		/// </summary>
		public TextNotNull NombreEmpleado { get; private set; }
		/// <summary>
		/// Apellido del empleado
		/// </summary>
		public TextNotNull ApellidoEmpleado { get; private set; }
		/// <summary>
		/// Fecha del permiso
		/// </summary>
		public DateTime FechaPermiso { get; private set; }


		/// <summary>
		/// Relacion con el tipo de permiso
		/// </summary>
		public PermissionType PermissionTypes { get; set; }


		/// <summary>
		/// Método con lógica de negocio para el update de permisos
		/// </summary>
		public static Permission UpdatePermission(Guid id, Guid tipoPermiso, TextNotNull nombreEmpleado, TextNotNull apellidoEmpleado, DateTime fechaPermiso)
		{
			return new Permission(new PermissionId(id), new PermissionTypeId(tipoPermiso), nombreEmpleado, apellidoEmpleado, fechaPermiso);
		}
	}
}