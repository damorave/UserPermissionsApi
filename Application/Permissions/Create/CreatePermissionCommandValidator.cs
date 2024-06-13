using FluentValidation;

namespace Application.Permissions.Create
{
	/// <summary>
	/// Capa de validacion con Fluent con el fin de mitrigar reglas de negocio en el modelo
	/// </summary>
	public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
	{
		public CreatePermissionCommandValidator()
		{
			RuleFor(r => r.TipoPermiso)
				.NotEmpty()
				.WithName("Tipo Permiso");

			RuleFor(r => r.NombreEmpleado)
				.NotEmpty()
				.MaximumLength(50)
				.WithName("Nombre Empleado");

			RuleFor(r => r.ApellidoEmpleado)
				.NotEmpty()
				.MaximumLength(50)
				.WithName("Apellido Empleado");
		}
	}
}
