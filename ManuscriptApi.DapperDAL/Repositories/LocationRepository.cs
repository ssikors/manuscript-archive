
using System.Data;
using Dapper;

namespace ManuscriptApi.DapperDAL
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IDbConnection _connection;

        public LocationRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Location> CreateAsync(Location model)
        {
            var sql = @"
        INSERT INTO Locations (Name, CountryId) 
        VALUES (@Name, @CountryId); 
        SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, model);
            model.Id = newId;

            return await GetByIdAsync(newId) ?? model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Locations WHERE Id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<List<Location>> GetAllAsync()
        {
            var sql = @"
        SELECT l.*, c.Id, c.Name, c.Description, c.IconUrl 
        FROM Locations l
        INNER JOIN Countries c ON l.CountryId = c.Id";

            var locationDict = new Dictionary<int, Location>();

            var locations = await _connection.QueryAsync<Location, Country, Location>(
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
            var sql = @"
        SELECT l.*, c.Id, c.Name, c.Description, c.IconUrl
        FROM Locations l
        INNER JOIN Countries c ON l.CountryId = c.Id
        WHERE l.Id = @Id";

            var location = await _connection.QueryAsync<Location, Country, Location>(
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
            var sql = @"
        UPDATE Locations 
        SET Name = @Name, 
            CountryId = @CountryId
        WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
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

}
