using CrudWithAuth.Entitites;
using CrudWithAuth.Model.DTO;
using CrudWithAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudWithAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Username already exist");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
            {
                return BadRequest("Invalid Username or Password");
            }

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);

            if (result is null || result.RefreshToken is null || result.AccessToken is null)
            {
                return BadRequest("Invalid refresh token");
            }

            return Ok(result);
        }


        [Authorize]
        [HttpGet]
        public IActionResult AuthenticateOnlyEndpoint()
        {
            return Ok("You're good");
        }

        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly() => Ok("You're good");
    }
}
