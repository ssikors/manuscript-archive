
namespace ManuscriptApi.Business.DTOs
{
    public class UserRegisterDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool IsModerator { get; set; }

        public required string Password { get; set; }
    }
}
