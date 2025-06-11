using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.MediatR.Commands;
using ManuscriptApi.Business.MediatR.Queries;
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
        [HttpPost("users")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            var id = await _sender.Send(command);

            if (id == 0)
            {
                return Unauthorized("Failed to register");
            }

            return Ok(id);
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
                return Unauthorized("Invalid username or password");
            }

            return Ok(token);
        }
    }
}
