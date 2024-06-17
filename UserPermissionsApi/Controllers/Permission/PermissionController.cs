using Application.Permissions.Create;
using Application.Permissions.GetAll;
using Application.Permissions.Update;
using Application.PermissionTypes.GetAll;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserPermissionsApi.Controllers.Permission
{
	/// <summary>
	/// Controlador encargado de recibir las peticiones para Permission
	/// </summary>
	[Route("permission")]
	public class PermissionController : ApiController
	{
		/// <summary>
		/// Send a request through the mediator pipeline to be handled by a single handler.
		/// </summary>
		private readonly ISender _mediator;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		public PermissionController(ISender mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Método controller encargado de crear permisos
		/// </summary>
		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> RequestPermission([FromBody] CreatePermissionCommand command)
		{
			var createPermissionresult = await _mediator.Send(command);

			// Dependiendo de la respuesta se encapsula en el Problem el cual se va a la factory PermissionProblemDetailsFactory dependiendo del tipo de error 
			return createPermissionresult.Match(
				permission => Ok(),
				errors => Problem(errors)
				);
		}

		/// <summary>
		/// Método controller encargado de obtener todos los permisos
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("getall")]
		public async Task<IActionResult> GetAllPermissions()
		{
			var permissionResult = await _mediator.Send(new GetAllPermissionsQuery());

			return permissionResult.Match(
				permission => Ok(permissionResult.Value),
				errors => Problem(errors)
				);
		}

		/// <summary>
		/// Método encargado de obtener un tipo de permiso por ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetPermissionById(Guid id)
		{
			var customerResult = await _mediator.Send(new GetPermissionByIdQuery(id));

			return customerResult.Match(
				customer => Ok(customer),
				errors => Problem(errors)
			);
		}

		/// <summary>
		/// Método encargado de actualizar un permiso
		/// </summary>
		/// <returns></returns>
		[HttpPut]
		[Route("update")]
		public async Task<IActionResult> ModifyPermission(Guid id, [FromBody] UpdatePermissionsCommand command)
		{
			if (command.Id != id)
			{
				List<Error> errors = new() {
					Error.Validation("Permission.UpdateInvalid", "The request Id does not match with the url id")
				};

				return Problem(errors);
			}

			var updateResult = await _mediator.Send(command);

			return updateResult.Match(
				permissionId => NoContent(),
				errors => Problem(errors)
				);
		}
	}
}
