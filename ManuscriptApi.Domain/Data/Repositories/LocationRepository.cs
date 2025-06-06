
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ManuscriptApi.DataAccess.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IConfiguration _configuration;

        public LocationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Location> CreateAsync(Location model)
        {
            using (var connection = GetConnection())
            {
                var sql = @"
                INSERT INTO Locations (Name, CountryId) 
                VALUES (@Name, @CountryId); 
                SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, model);
                model.Id = newId;

                return await GetByIdAsync(newId) ?? model;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = GetConnection())
            {
                var sql = "DELETE FROM Locations WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<List<Location>> GetAllAsync()
        {
            using var connection = GetConnection();

            var sql = @"
            SELECT l.*, c.Id, c.Name, c.Description, c.IconUrl 
            FROM Locations l
            INNER JOIN Countries c ON l.CountryId = c.Id";

            var locationDict = new Dictionary<int, Location>();

            var locations = await connection.QueryAsync<Location, Country, Location>(
                sql,
                (location, country) =>
                {
                    if (!locationDict.TryGetValue(location.Id, out var loc))
                    {
                        loc = location;
                        loc.Country = country;
                        locationDict.Add(loc.Id, loc);
                    }
                    return loc;
                },
                splitOn: "Id");

            return locationDict.Values.ToList();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var sql = @"
            SELECT l.*, c.Id, c.Name, c.Description, c.IconUrl
            FROM Locations l
            INNER JOIN Countries c ON l.CountryId = c.Id
            WHERE l.Id = @Id";

            var location = await connection.QueryAsync<Location, Country, Location>(
                sql,
                (loc, country) =>
                {
                    loc.Country = country;
                    return loc;
                },
                new { Id = id },
                splitOn: "Id");

            return location.FirstOrDefault();
        }


        public async Task<Location?> UpdateAsync(Location model, int id)
        {
            using (var connection = GetConnection())
            {
                var sql = @"
                UPDATE Locations 
                SET Name = @Name, 
                    CountryId = @CountryId
                WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    model.Name,
                    model.CountryId,
                    Id = id
                });

                if (rowsAffected == 0)
                    return null;

                return await GetByIdAsync(id);
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
