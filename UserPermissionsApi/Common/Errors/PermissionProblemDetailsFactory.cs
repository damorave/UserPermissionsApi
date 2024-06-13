using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using UserPermissionsApi.Common.Http;

namespace UserPermissionsApi.Common.Errors
{
	/// <summary>
	/// Factoria encargada de los manejadores de excepciones a nivel de peticiones de la API
	/// </summary>
	public class PermissionProblemDetailsFactory : ProblemDetailsFactory
	{
		/// <summary>
		/// Options used to configure behavior for types annotated with Microsoft.AspNetCore.Mvc.ApiControllerAttribute.
		/// </summary>
		private readonly ApiBehaviorOptions _options;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public PermissionProblemDetailsFactory(ApiBehaviorOptions options)
		{
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public override ProblemDetails CreateProblemDetails(
			HttpContext httpContext,
			int? statusCode = null,
			string? title = null,
			string? type = null,
			string? detail = null,
			string? instance = null)
		{
			statusCode ??= 500;

			var problemDetails = new ProblemDetails
			{
				Status = statusCode,
				Title = title,
				Type = type,
				Detail = detail,
				Instance = instance
			};

			ApplyProblemDetailsDefault(httpContext, problemDetails, statusCode.Value);

			return problemDetails;
		}

		public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext,
			ModelStateDictionary modelStateDictionary,
			int? statusCode = null,
			string? title = null,
			string? type = null,
			string? detail = null,
			string? instance = null)
		{
			if (modelStateDictionary == null)
			{
				throw new ArgumentNullException(nameof(modelStateDictionary));
			}

			statusCode ??= 400;

			var problemDetails = new ValidationProblemDetails(modelStateDictionary)
			{
				Status = statusCode,
				Type = type,
				Detail = detail,
				Instance = instance
			};

			if (title != null)
			{
				problemDetails.Title = title;
			}

			ApplyProblemDetailsDefault(httpContext, problemDetails, statusCode.Value);

			return problemDetails;

		}

		/// <summary>
		/// Método encargado del mapeo correcto de los errores http
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="problemDetails"></param>
		/// <param name="statusCode"></param>
		private void ApplyProblemDetailsDefault(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
		{
			problemDetails.Status ??= statusCode;

			if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
			{
				problemDetails.Title ??= clientErrorData.Title;
				problemDetails.Type ??= clientErrorData.Link;
			}

			var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

			if (traceId != null)
			{
				problemDetails.Extensions["traceId"] = traceId;
			}

			var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;

			if (errors is not null)
			{
				problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
			}
		}
	}
}
