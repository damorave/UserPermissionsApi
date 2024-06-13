using Application.Data;
using Domain.Permissions;
using Domain.PermissionTypes;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
	{
		/// <summary>
		/// Asynchronously send a notification to multiple handlers
		/// Publicación de los eventos de dominio
		/// </summary>
		private IPublisher _publisher;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		/// <param name="publisher"></param>
		public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options)
		{
			_publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
		}

		public DbSet<Permission> Permission { get; set; }
		public DbSet<PermissionType> PermissionType { get; set; }

		/// <summary>
		/// Se aplican configuraciones de entidades customizadas
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}

		/// <summary>
		/// Su objetivo es tomar los eventos emitidos para así recorrerlos uno a uno y emitirlos
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			var domainEvents = ChangeTracker.Entries<AggregateRoot>()
				.Select(o => o.Entity)
				.Where(o => o.GetDomainEvents().Any())
				.SelectMany(o => o.GetDomainEvents());

			var result = await base.SaveChangesAsync(cancellationToken);

			foreach (var domainEvent in domainEvents)
			{
				await _publisher.Publish(domainEvent, cancellationToken);
			}

			return result;
		}
	}
}
