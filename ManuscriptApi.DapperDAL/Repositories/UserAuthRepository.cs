using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ManuscriptApi.Domain.Models;

namespace ManuscriptApi.DapperDAL.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly IDbConnection _db;

        public UserAuthRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> InsertUserAuth(UserAuth userAuth)
        {
            const string sql = @"
            INSERT INTO [UserAuth] ([UserId], [PasswordHash])
            VALUES (@UserId, @PasswordHash);

            SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var newId = await _db.QuerySingleAsync<int>(sql, new
            {
                userAuth.UserId,
                userAuth.PasswordHash
            });

            return newId;
        }

        public async Task<UserAuth?> GetUserAuth(int userId)
        {
            const string sql = "SELECT * FROM UserAuth WHERE UserId = @UserId";

            var userAuth = await _db.QueryFirstOrDefaultAsync<UserAuth>(sql, new { UserId = userId });

            return userAuth;
        }
    }
}
