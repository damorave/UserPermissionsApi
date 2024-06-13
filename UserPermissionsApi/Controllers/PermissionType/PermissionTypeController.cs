using Application.PermissionTypes.Create;
using Application.PermissionTypes.GetAll;
using Application.PermissionTypes.Update;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UserPermissionsApi.Controllers.PermissionType
{
	/// <summary>
	/// Controlador encargado de recibir las peticiones para Permission
	/// </summary>
	[Route("permissionTypes")]
	public class PermissionTypeController : ApiController
	{
		/// <summary>
		/// Send a request through the mediator pipeline to be handled by a single handler.
		/// </summary>
		private readonly ISender _mediator;

		/// <summary>
		/// Crea una nueva instancia de la clase
		/// </summary>
		/// <param name="mediator"></param>
		public PermissionTypeController(ISender mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		/// <summary>
		/// Método controller encargado de crear tipos de permisos
		/// </summary>
		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> RequestPermissionType([FromBody] CreatePermissionTypesCommand command)
		{
			var createPermissionTyperesult = await _mediator.Send(command);

			// Dependiendo de la respuesta se encapsula en el Problem el cual se va a la factory PermissionProblemDetailsFactory dependiendo del tipo de error 
			return createPermissionTyperesult.Match(
				permission => Ok(),
				errors => Problem(errors)
				);
		}

		/// <summary>
		/// Método encargado de obtener un tipo de permiso por ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetPermissionTypeById(Guid id)
		{
			var customerResult = await _mediator.Send(new GetPermissionTypeByIdQuery(id));

			return customerResult.Match(
				customer => Ok(customer),
				errors => Problem(errors)
			);
		}

		/// <summary>
		/// Método controller encargado de obtener todos los tipos de permisos
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("getall")]
		public async Task<IActionResult> GetAllPermissionsTypes()
		{
			var permissionResult = await _mediator.Send(new GetAllPermissionTypesQuery());

			return permissionResult.Match(
				permission => Ok(permissionResult.Value),
				errors => Problem(errors)
				);
		}

		/// <summary>
		/// Método encargado de actualizar un tipo de permiso
		/// </summary>
		/// <param name="id"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("update/{id}")]
		public async Task<IActionResult> ModifyPermissionType(Guid id, [FromBody] UpdatePermissionsTypeCommand command)
		{
			if (command.Id != id)
			{
				List<Error> errors = new() {
					Error.Validation("PermissionType.UpdateInvalid", "The request Id does not match with the url id")
				};

				return Problem(errors);
			}

			var updateResult = await _mediator.Send(command);

			return updateResult.Match(
				permissionTypeId => NoContent(),
				errors => Problem(errors)
				);
		}
	}
}
