using FluentValidation;

namespace Application.PermissionTypes.Create
{
	/// <summary>
	/// Capa de validacion con Fluent con el fin de mitrigar reglas de negocio en el modelo
	/// </summary>
	public class CreatePermissionTypesCommandValidator : AbstractValidator<CreatePermissionTypesCommand>
	{
		public CreatePermissionTypesCommandValidator()
		{
			RuleFor(r => r.Descripcion)
				.NotEmpty()
				.MaximumLength(80);
		}
	}
}
