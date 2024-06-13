using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public static class DependencyInjection
	{
		/// <summary>
		/// Clase encargada de realizar la inyección de dependencias.
		/// </summary>
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
			});

			services.AddScoped(
				typeof(IPipelineBehavior<,>),
				typeof(ValidationBehavior<,>)
				);

			// Se agrega validaciones para los ensambladores
			services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();


			return services;
		}
	}
}
