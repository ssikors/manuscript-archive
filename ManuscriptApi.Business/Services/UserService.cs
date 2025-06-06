using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Business.Services
{
    public class UserService : DapperCrudService<User>
    {
        public UserService(IUserRepository repository) : base(repository)
        {
        }
    }
}
