
using System.Data;
using Dapper;

namespace ManuscriptApi.DapperDAL
{
    public class TagRepository : ITagRepository
    {
        private readonly IDbConnection _connection;

        public TagRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Tag> CreateAsync(Tag model)
        {
            var sql = @"
        INSERT INTO Tags (Name, Description, IsDeleted) 
        VALUES (@Name, @Description, @IsDeleted); 
        SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, model);

            model.Id = newId;

            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "UPDATE Tags SET IsDeleted = 1 WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            var sql = "SELECT * FROM Tags WHERE IsDeleted IS NULL OR IsDeleted = 0";

            var tags = await _connection.QueryAsync<Tag>(sql);

            return tags.ToList();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Tags WHERE Id = @Id AND (IsDeleted IS NULL OR IsDeleted = 0)";

            var tag = await _connection.QueryFirstOrDefaultAsync<Tag>(sql, new { Id = id });

            return tag;
        }

        public async Task<Tag?> UpdateAsync(Tag model, int id)
        {
            var sql = @"
        UPDATE Tags
        SET Name = @Name,
            Description = @Description,
            IsDeleted = @IsDeleted
        WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
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
    }
}
