
using ManuscriptApi.Domain.Models;

namespace ManuscriptApi.DapperDAL.Repositories
{
    public interface IUserAuthRepository
    {
        Task<int> InsertUserAuth(UserAuth userAuth);
        Task<UserAuth?> GetUserAuth(int userId);
    }
}
