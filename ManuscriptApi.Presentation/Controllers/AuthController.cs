
using ManuscriptApi.Business.Commands;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.Queries;
using ManuscriptApi.Business.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Endpoint for registering an account
        /// </summary>
        /// <returns>Status 200 on successful registration</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            var id = await _sender.Send(command);

            if (id == 0)
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
        public async Task<ActionResult<string>> Login(LoginUserQuery query)
        {
            var token = await _sender.Send(query);

            if (token == null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);
        }
    }
}
