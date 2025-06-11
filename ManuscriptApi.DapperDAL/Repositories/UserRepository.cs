
using System.Data;
using Dapper;

namespace ManuscriptApi.DapperDAL
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> CreateAsync(User model)
        {
            var sql = @"
            INSERT INTO Users (Username, Email, IsModerator, IsDeleted)
            VALUES (@Username, @Email, @IsModerator, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, model);

            model.Id = newId;
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "UPDATE Users SET IsDeleted = 1 WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users WHERE IsDeleted IS NULL OR IsDeleted = 0";

            var users = await _connection.QueryAsync<User>(sql);

            return users.ToList();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var user = await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });

            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var user = await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });

            return user;
        }

        public async Task<User?> UpdateAsync(User model, int id)
        {
            var sql = @"
            UPDATE Users
            SET Username = @Username,
            Email = @Email,
            IsModerator = @IsModerator,
            IsDeleted = @IsDeleted
            WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                model.Username,
                model.Email,
                model.IsModerator,
                model.IsDeleted,
                Id = id
            });

            if (rowsAffected == 0)
                return null;

            model.Id = id;
            return model;
        }
    }

}
