using AuthenticationJwt.Web.Models;
using AuthenticationJwt.Web.Service;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationJwt.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LogInController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginModel user)
        {
            var response = await _authService.LoginAsync(user);
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost, Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserModel user)
        {
            var response = await _authService.RegisterUser(user);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
