using ErrorOr;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
	/// <summary>
	/// Clase encargada de agregar una capa adicional de validaciones para los handler
	/// el TRequest es de MeditR pero el TResponse es de ErrorOr
	/// </summary>
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : IErrorOr
	{
		/// <summary>
		/// Defines a validator for a particular type.
		/// </summary>
		private readonly IValidator<TRequest> _validator;


		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public ValidationBehavior(IValidator<TRequest>? validator = null)
		{
			_validator = validator;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			if (_validator is null)
			{
				return await next();
			}

			// Resultados de validaciones con Fluent
			var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

			if (validatorResult.IsValid)
			{
				return await next();
			}

			// Se convierten los errores de FluentValidation a errores de tipo ErrorOr
			var errors = validatorResult.Errors
				.ConvertAll(validationResultFailure => Error.Validation(
					validationResultFailure.PropertyName,
					validationResultFailure.ErrorMessage));

			return (dynamic)errors;
		}
	}
}
