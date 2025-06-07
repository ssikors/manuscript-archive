
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Presentation.Controllers
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

        /// <summary>
        /// Endpoint for registering an account
        /// </summary>
        /// <returns>Status 200 on successful registration</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            var id = await _authService.RegisterAsync(request);

            if (id == null)
            {
                return BadRequest("Failed to register");
            }

            return Ok();
        }

        /// <summary>
        /// Endpoint for log in
        /// </summary>
        /// <returns>A jwt token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto req)
        {
            var token = await _authService.LoginAsync(req);
            if (token == null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);
        }
    }
}
