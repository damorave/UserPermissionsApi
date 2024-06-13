using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class InitialMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "PermissionType",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Descripcion = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PermissionType", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Permission",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TipoPermiso = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					NombreEmpleado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					ApellidoEmpleado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
					FechaPermiso = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Permission", x => x.Id);
					table.ForeignKey(
						name: "FK_Permission_PermissionType_TipoPermiso",
						column: x => x.TipoPermiso,
						principalTable: "PermissionType",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_Permission_TipoPermiso",
				table: "Permission",
				column: "TipoPermiso");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Permission");

			migrationBuilder.DropTable(
				name: "PermissionType");
		}
	}
}
