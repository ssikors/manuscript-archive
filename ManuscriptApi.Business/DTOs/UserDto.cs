namespace ManuscriptApi.Business.DTOs
{
    public class UserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsModerator { get; set; }
    }

}
