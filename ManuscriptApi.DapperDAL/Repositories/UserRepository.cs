
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ManuscriptApi.DapperDAL
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<User> CreateAsync(User model)
        {
            using var connection = GetConnection();

            var sql = @"
            INSERT INTO Users (Username, Email, IsModerator, IsDeleted)
            VALUES (@Username, @Email, @IsModerator, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await connection.ExecuteScalarAsync<int>(sql, model);

            model.Id = newId;
            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var sql = "DELETE FROM Users WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<User>> GetAllAsync()
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Users WHERE IsDeleted IS NULL OR IsDeleted = 0";

            var users = await connection.QueryAsync<User>(sql);

            return users.ToList();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Users WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });

            return user;
        }

        public async Task<User?> UpdateAsync(User model, int id)
        {
            using var connection = GetConnection();

            var sql = @"
            UPDATE Users
            SET Username = @Username,
                Email = @Email,
                IsModerator = @IsModerator,
                IsDeleted = @IsDeleted
            WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new
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

        private SqlConnection GetConnection()
            => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

}
