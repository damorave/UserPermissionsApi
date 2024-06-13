using Domain.Helpers.Identify;
using Domain.PermissionTypes;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
	/// <summary>
	/// Configuracion de las propiedades para la migración
	/// objeto de dominio a entidad de DB
	/// </summary>
	public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
	{
		public void Configure(EntityTypeBuilder<PermissionType> builder)
		{
			builder.ToTable("PermissionType");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id).HasConversion(
				permissionId => permissionId.Value,
				value => new PermissionTypeId(value)
				);

			builder.Property(x => x.Descripcion).HasConversion(
				descripcion => descripcion.Value,
				value => TextNotNull.Validate(value)!).HasMaxLength(80);
		}
	}
}
