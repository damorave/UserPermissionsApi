using Domain.Helpers.Identify;
using Domain.Permissions;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
	/// <summary>
	/// Configuracion de las propiedades para la migración
	/// objeto de dominio a entidad de DB
	/// </summary>
	public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
	{
		public void Configure(EntityTypeBuilder<Permission> builder)
		{
			builder.ToTable("Permission");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).HasConversion(
				permissionId => permissionId.Value,
				value => new PermissionId(value)
				);

			builder.Property(x => x.TipoPermiso).HasConversion(
				tipoPermiso => tipoPermiso.Value,
				value => new PermissionTypeId(value)
				);

			builder.Property(x => x.NombreEmpleado).HasConversion(
				nombreEmpleado => nombreEmpleado.Value,
				value => TextNotNull.Validate(value)!).HasMaxLength(50);

			builder.Property(x => x.ApellidoEmpleado).HasConversion(
				apellidoEmpleado => apellidoEmpleado.Value,
				value => TextNotNull.Validate(value)!).HasMaxLength(50);

			builder.Property(x => x.FechaPermiso);

			builder.HasOne(x => x.PermissionTypes).WithMany(p => p.Permissions).HasForeignKey(x => x.TipoPermiso).OnDelete(DeleteBehavior.NoAction).IsRequired();

		}
	}
}
