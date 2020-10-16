using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Services;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService) => this.userService = userService;

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto data)
        {
            return (await userService.CreateUser(data)).GetActionResult(this);
        }

        [AllowAnonymous]
        [HttpPost("confirm")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Confirm([FromBody] ConfirmUserDto data)
        {
            return (await userService.ConfirmUser(data)).GetActionResult(this);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(AuthenticateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Authenticate([FromBody] VerifyPasswordDto data)
        {
            return (await userService.VerifyPassword(data)).GetActionResult(this);
        }

        [AllowAnonymous]
        [HttpPost("resetPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ResetPassword([FromBody] UserData data)
        {
            return (await userService.ResetPassword(data.Email)).GetActionResult(this);
        }

        [HttpPost("changePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordDto data)
        {
            return (await userService.ChangePassword(data)).GetActionResult(this);
        }

        [AllowAnonymous]
        [HttpPost("changeResetPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordResetDto data)
        {
            return (await userService.ChangePassword(data)).GetActionResult(this);
        }
    }
}
