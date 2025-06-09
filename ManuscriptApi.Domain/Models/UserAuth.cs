namespace ManuscriptApi.Domain.Models
{
    public class UserAuth
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

    }
}
