using System.Data;
using Dapper;

namespace ManuscriptApi.DapperDAL
{
    public class ManuscriptRepository : IManuscriptRepository
    {
        private readonly IDbConnection _connection;

        public ManuscriptRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Manuscript> CreateAsync(Manuscript model)
        {
            var sql = @"
        INSERT INTO Manuscripts (
            Title, Description, YearWrittenStart, YearWrittenEnd, 
            SourceUrl, CreatedAt, LocationId, AuthorId, IsDeleted
        )
        VALUES (
            @Title, @Description, @YearWrittenStart, @YearWrittenEnd,
            @SourceUrl, @CreatedAt, @LocationId, @AuthorId, @IsDeleted
        );
        SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, model);
            model.Id = newId;

            if (model.Tags?.Any() == true)
            {
                var tagParams = model.Tags.Select(tag => new
                {
                    ManuscriptId = newId,
                    TagId = tag.Id
                });

                await _connection.ExecuteAsync(
                    "INSERT INTO ManuscriptTag (ManuscriptId, TagId) VALUES (@ManuscriptId, @TagId);",
                    tagParams
                );
            }

            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Manuscripts WHERE Id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<Manuscript>> GetAllAsync()
        {
            var sql = @"
        SELECT m.*, 
               u.Id, u.Username, u.Email, u.IsModerator, u.IsDeleted,
               l.Id, l.Name, l.CountryId,
               t.Id, t.Name, t.Description, t.IsDeleted,
               i.Id, i.Title, i.Url, i.ManuscriptId, i.IsDeleted
        FROM Manuscripts m
        INNER JOIN Users u ON m.AuthorId = u.Id
        INNER JOIN Locations l ON m.LocationId = l.Id
        LEFT JOIN ManuscriptTag mt ON m.Id = mt.ManuscriptId
        LEFT JOIN Tags t ON mt.TagId = t.Id
        LEFT JOIN Images i ON i.ManuscriptId = m.Id
        WHERE m.IsDeleted IS NULL OR m.IsDeleted = 0";

            var manuscriptDict = new Dictionary<int, Manuscript>();

            var manuscripts = await _connection.QueryAsync<
                Manuscript, User, Location, Tag, Image, Manuscript>(
                sql,
                (manuscript, user, location, tag, image) =>
                {
                    if (!manuscriptDict.TryGetValue(manuscript.Id, out var m))
                    {
                        m = manuscript;
                        m.Author = user;
                        m.Location = location;
                        m.Tags = new List<Tag>();
                        m.Images = new List<Image>();
                        manuscriptDict[m.Id] = m;
                    }

                    if (tag != null && !m.Tags.Any(t => t.Id == tag.Id))
                        m.Tags.Add(tag);

                    if (image != null && !m.Images.Any(i => i.Id == image.Id))
                        m.Images.Add(image);

                    return m;
                },
                splitOn: "Id,Id,Id,Id");

            return manuscriptDict.Values.ToList();
        }

        public async Task<Manuscript?> GetByIdAsync(int id)
        {
            var sql = @"
        SELECT m.*, 
               u.Id, u.Username, u.Email, u.IsModerator, u.IsDeleted,
               l.Id, l.Name, l.CountryId,
               t.Id, t.Name, t.Description, t.IsDeleted,
               i.Id, i.Title, i.Url, i.ManuscriptId, i.IsDeleted
        FROM Manuscripts m
        INNER JOIN Users u ON m.AuthorId = u.Id
        INNER JOIN Locations l ON m.LocationId = l.Id
        LEFT JOIN ManuscriptTag mt ON m.Id = mt.ManuscriptId
        LEFT JOIN Tags t ON mt.TagId = t.Id
        LEFT JOIN Images i ON i.ManuscriptId = m.Id
        WHERE m.Id = @Id AND (m.IsDeleted IS NULL OR m.IsDeleted = 0)";

            var manuscriptDict = new Dictionary<int, Manuscript>();

            var manuscripts = await _connection.QueryAsync<
                Manuscript, User, Location, Tag, Image, Manuscript>(
                sql,
                (manuscript, user, location, tag, image) =>
                {
                    if (!manuscriptDict.TryGetValue(manuscript.Id, out var m))
                    {
                        m = manuscript;
                        m.Author = user;
                        m.Location = location;
                        m.Tags = new List<Tag>();
                        m.Images = new List<Image>();
                        manuscriptDict[m.Id] = m;
                    }

                    if (tag != null && !m.Tags.Any(t => t.Id == tag.Id))
                        m.Tags.Add(tag);

                    if (image != null && !m.Images.Any(i => i.Id == image.Id))
                        m.Images.Add(image);

                    return m;
                },
                new { Id = id },
                splitOn: "Id,Id,Id,Id");

            return manuscriptDict.Values.FirstOrDefault();
        }

        public async Task<Manuscript?> UpdateAsync(Manuscript model, int id)
        {
            var sql = @"
        UPDATE Manuscripts
        SET Title = @Title,
            Description = @Description,
            YearWrittenStart = @YearWrittenStart,
            YearWrittenEnd = @YearWrittenEnd,
            SourceUrl = @SourceUrl,
            CreatedAt = @CreatedAt,
            LocationId = @LocationId,
            AuthorId = @AuthorId,
            IsDeleted = @IsDeleted
        WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                model.Title,
                model.Description,
                model.YearWrittenStart,
                model.YearWrittenEnd,
                model.SourceUrl,
                model.CreatedAt,
                model.LocationId,
                model.AuthorId,
                model.IsDeleted,
                Id = id
            });

            if (rowsAffected == 0)
                return null;

            if (model.Tags?.Any() == true)
            {
                var tagParams = model.Tags.Select(tag => new
                {
                    ManuscriptId = id,
                    TagId = tag.Id
                });

                await _connection.ExecuteAsync(
                    "INSERT INTO ManuscriptTag (ManuscriptId, TagId) VALUES (@ManuscriptId, @TagId);",
                    tagParams
                );
            }


            model.Id = id;
            return model;
        }
    }

}
