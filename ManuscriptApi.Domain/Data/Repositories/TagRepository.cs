
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ManuscriptApi.DataAccess.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IConfiguration _configuration;

        public TagRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Tag> CreateAsync(Tag model)
        {
            using var connection = GetConnection();

            var sql = @"
            INSERT INTO Tags (Name, Description, IsDeleted) 
            VALUES (@Name, @Description, @IsDeleted); 
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await connection.ExecuteScalarAsync<int>(sql, model);

            model.Id = newId;

            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var sql = "DELETE FROM Tags WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Tags WHERE IsDeleted IS NULL OR IsDeleted = 0";

            var tags = await connection.QueryAsync<Tag>(sql);

            return tags.ToList();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var sql = "SELECT * FROM Tags WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var tag = await connection.QueryFirstOrDefaultAsync<Tag>(sql, new { Id = id });

            return tag;
        }

        public async Task<Tag?> UpdateAsync(Tag model, int id)
        {
            using var connection = GetConnection();

            var sql = @"
            UPDATE Tags
            SET Name = @Name,
                Description = @Description,
                IsDeleted = @IsDeleted
            WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                model.Name,
                model.Description,
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
