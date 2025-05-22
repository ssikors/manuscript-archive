using ManuscriptApi.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManuscriptApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "userName2143", Email = "user@email.test", isModerator = false },
            new User { Id = 2, Name = "john1234", Email = "user@email.test", isModerator = false },
        };


        /// <summary>
        /// Endpoint for getting all users
        /// </summary>
        /// <returns>A list of User objects</returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Users);
        }

        /// <summary>
        /// Endpoint for adding a User
        /// </summary>
        [HttpPost]
        public ActionResult<User> AddUser(UserDto userDto)
        {
            User user = new User
            {
                Id = Users.Max(c => c.Id) + 1,
                Name = userDto.Name,
                Email = userDto.Email,
                isModerator = userDto.isModerator
            };

      
            Users.Add(user);

            return CreatedAtAction(nameof(AddUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Endpoint for getting a User by their id
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            User? user = Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Endpoint for updating a User specified by id
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, User updatedUser)
        {
            User? user = Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.isModerator = updatedUser.isModerator;

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a User specified by id
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            User? user = Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            Users.Remove(user);
            return NoContent();
        }
    }
}
