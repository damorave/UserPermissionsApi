using Domain.Helpers.Identify;
using Domain.Permissions;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.PermissionTypes
{
	public sealed class PermissionType : AggregateRoot
	{
		/// <summary>
		/// Crea una nueva instancia de la clase, seteo de propiedades
		/// </summary>
		/// <param name="id"></param>
		/// <param name="descripcion"></param>
		public PermissionType(PermissionTypeId id, TextNotNull descripcion)
		{
			Id = id;
			Descripcion = descripcion;
		}

		public PermissionType()
		{

		}


		/// <summary>
		/// Id del Permiso
		/// </summary>
		public PermissionTypeId Id { get; private set; }
		/// <summary>
		/// Descripción del tipo de permiso
		/// </summary>
		public TextNotNull Descripcion { get; private set; }

		/// <summary>
		/// Relación del Permission con el PermissionType
		/// </summary>
		public List<Permission> Permissions { get; set; } = new List<Permission>();

		/// <summary>
		/// Método con lógica de negocio para el update de tipos permisos
		/// </summary>
		public static PermissionType UpdatePermissionType(Guid id, TextNotNull Descripcion)
		{
			return new PermissionType(new PermissionTypeId(id), Descripcion);
		}
	}
}
