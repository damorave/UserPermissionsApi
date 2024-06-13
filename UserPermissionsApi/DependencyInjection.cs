using UserPermissionsApi.Middlewares;

namespace UserPermissionsApi
{
	public static class DependencyInjection
	{
		/// <summary>
		/// Clase encargada de realizar la inyección de dependencias.
		/// </summary>
		public static IServiceCollection AddPresentations(this IServiceCollection services)
		{
			services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddTransient<GlobalExceptionHandlerMiddlewares>();

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
					.WithOrigins("http://localhost:5173")
						.AllowAnyMethod()
						.AllowAnyHeader()
				);
			});

			return services;
		}
	}
}
