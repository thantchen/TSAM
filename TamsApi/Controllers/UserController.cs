using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TamsApi.Core.Identity;

namespace TamsApi.Controllers
{
    [ApiController]
    public class UserController : AuthenticatedControllerBase
    {
        public UserController(IAuthenticatedUserBuilder builder) : base(builder)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("api/user")]
        public  ActionResult<ApiResponse<AuthenticatedUser>> Show()
        {
            return Ok(new ApiResponse<AuthenticatedUser>(CurrentUser));
        }
    }
}
