
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace ManuscriptApi.DataAccess.Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IConfiguration _configuration;

        public CountryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Country> CreateAsync(Country model)
        {
            using (var connection = GetConnection())
            {
                var sql = @"
                INSERT INTO Countries (Name, Description, IconUrl) 
                VALUES (@Name, @Description, @IconUrl); 
                SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, model);

                model.Id = newId;

                return model;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = GetConnection())
            {
                var sql = "DELETE FROM Countries WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<List<Country>> GetAllAsync()
        {
            using (var connection = GetConnection())
            {
                var countries = await connection.QueryAsync<Country>("SELECT * FROM Countries");
                return countries.ToList();
            }
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            using (var connection = GetConnection())
            {
                var country = await connection.QueryFirstAsync<Country>("SELECT * FROM Countries WHERE ID = @Id", new { Id = id });
                return country;
            }
        }

        public async Task<Country?> UpdateAsync(Country model, int id)
        {
            using (var connection = GetConnection())
            {
                var sql = @"
                UPDATE Countries 
                SET Name = @Name, 
                Description = @Description, 
                IconUrl = @IconUrl 
                WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    model.Name,
                    model.Description,
                    model.IconUrl,
                    Id = id
                });

                if (rowsAffected == 0)
                    return null;

                model.Id = id;
                return model;
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
