using ManuscriptApi.DapperDAL;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.Services
{
    public class UserService : DapperCrudService<User>
    {
        public UserService(IUserRepository repository, ILogger<DapperCrudService<User>> logger) : base(repository, logger)
        {
        }
    }
}
