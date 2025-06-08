using AutoMapper;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Business.MediatR.Queries;
using ManuscriptApi.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Moderator)]
    public class UsersController : ControllerBase
    {
        private readonly ICrudService<User> _userService;
        private readonly IMapper _mapper;

        public UsersController(ICrudService<User> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Endpoint for getting all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Endpoint for adding a User
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }

        /// <summary>
        /// Endpoint for getting a User by their id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Endpoint for updating a User specified by id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserDto updatedUser)
        {
            var entity = _mapper.Map<User>(updatedUser);
            var updated = await _userService.UpdateAsync(entity, id);

            if (updated == null)
                return BadRequest("Could not update the user");

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a User specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteAsync(id);

            if (!deleted)
                return BadRequest("Could not delete the user");

            return NoContent();
        }
    }
}
