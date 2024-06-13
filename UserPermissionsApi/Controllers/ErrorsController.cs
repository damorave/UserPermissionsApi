using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserPermissionsApi.Controllers
{
	public class ErrorsController : ControllerBase
	{
		/// Excluido del swagger
		[ApiExplorerSettings(IgnoreApi = true)]
		[Route("/error")]
		public IActionResult Error()
		{
			Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

			return Problem();
		}
	}
}
