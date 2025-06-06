using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ManuscriptApi.DapperDAL
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IDbConnection _connection;
        private readonly IConfiguration _configuration;

        public CountryRepository(IConfiguration configuration, IDbConnection connection)
        {
            _configuration = configuration;
            _connection = connection;
        }

        public async Task<Country> CreateAsync(Country model)
        {
            var sql = @"
            INSERT INTO Countries (Name, Description, IconUrl) 
            VALUES (@Name, @Description, @IconUrl); 
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, model);

            model.Id = newId;

            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "UPDATE Countries SET IsDeleted = 1 WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            var countries = await _connection.QueryAsync<Country>("SELECT * FROM Countries");
            return countries.ToList();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            var country = await _connection.QueryFirstAsync<Country>("SELECT * FROM Countries WHERE ID = @Id", new { Id = id });
            return country;
        }

        public async Task<Country?> UpdateAsync(Country model, int id)
        {
            var sql = @"
            UPDATE Countries 
            SET Name = @Name, 
            Description = @Description, 
            IconUrl = @IconUrl 
            WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
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
}
