using System.Security;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TamsApi.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("api/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (context.Error != null && context.Error is SecurityException)
            {
                return Unauthorized();
            }

            return Problem();
        }
    }
}
