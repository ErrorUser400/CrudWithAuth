using CrudWithAuth.Model;
using CrudWithAuth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudWithAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModelDTO login)
        {
            IActionResult response = Unauthorized();
            UserModel user = await _authService.AutheticateUser(login: login);

            if (user != null)
            {
                var tokenString = _authService.GenerateJsonWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return Ok(response);
        }
    }
}
