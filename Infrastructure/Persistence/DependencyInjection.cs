using Application.Data;
using Domain.Permissions;
using Domain.PermissionTypes;
using Domain.Primitives;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
	/// <summary>
	/// Inyección del DbContext
	/// </summary>
	public static class DependencyInjection
	{

		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddPersistence(configuration);
			return services;
		}

		private static IServiceCollection AddPersistence(this IServiceCollection service, IConfiguration configuration)
		{
			service.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DataBase")));
			service.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
			service.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

			service.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = "localhost:6379";
			});

			service.AddScoped<IPermissionRepository, PermissionRepository>();
			service.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();

			return service;
		}
	}
}
