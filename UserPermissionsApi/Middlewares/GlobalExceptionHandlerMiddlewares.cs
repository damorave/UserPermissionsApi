using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace UserPermissionsApi.Middlewares
{
	/// <summary>
	/// Manejador global de excepciones no controladas
	/// </summary>
	public class GlobalExceptionHandlerMiddlewares : IMiddleware

	{
		private readonly ILogger<GlobalExceptionHandlerMiddlewares> _logger;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		/// <param name="logger"></param>
		public GlobalExceptionHandlerMiddlewares(ILogger<GlobalExceptionHandlerMiddlewares> logger) => _logger = logger;

		/// <summary>
		/// Método encargado de manejar las peticiones http
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				ProblemDetails problem = new()
				{
					Status = (int)HttpStatusCode.InternalServerError,
					Type = "Server Error",
					Title = "Server Error",
					Detail = "An internal server has ocurred"
				};

				string json = JsonSerializer.Serialize(problem);

				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(json);
			}
		}
	}
}
