using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ManuscriptApi.DataAccess.Data.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Image> CreateAsync(Image model)
        {
            using var connection = GetConnection();

            var sql = @"
            INSERT INTO Images (Title, Url, ManuscriptId, IsDeleted)
            VALUES (@Title, @Url, @ManuscriptId, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await connection.ExecuteScalarAsync<int>(sql, model);
            model.Id = newId;

            if (model.Tags?.Any() == true)
            {
                foreach (var tag in model.Tags)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO ImageTag (ImageId, TagId) VALUES (@ImageId, @TagId);",
                        new { ImageId = newId, TagId = tag.Id }
                    );
                }
            }

            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = GetConnection();

            var sql = "DELETE FROM Images WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<Image>> GetAllAsync()
        {
            using var connection = GetConnection();

            var sql = @"
            SELECT i.*, t.Id, t.Name, t.Description, t.IsDeleted
            FROM Images i
            LEFT JOIN ImageTag it ON i.Id = it.ImageId
            LEFT JOIN Tags t ON it.TagId = t.Id
            WHERE i.IsDeleted IS NULL OR i.IsDeleted = 0";

            var imageDict = new Dictionary<int, Image>();

            var images = await connection.QueryAsync<Image, Tag, Image>(
                sql,
                (image, tag) =>
                {
                    if (!imageDict.TryGetValue(image.Id, out var img))
                    {
                        img = image;
                        img.Tags = new List<Tag>();
                        imageDict.Add(img.Id, img);
                    }

                    if (tag != null && !img.Tags.Any(t => t.Id == tag.Id))
                    {
                        img.Tags.Add(tag);
                    }

                    return img;
                },
                splitOn: "Id");

            return imageDict.Values.ToList();
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            using var connection = GetConnection();

            var sql = @"
            SELECT i.*, t.Id, t.Name, t.Description, t.IsDeleted
            FROM Images i
            LEFT JOIN ImageTag it ON i.Id = it.ImageId
            LEFT JOIN Tags t ON it.TagId = t.Id
            WHERE i.Id = @Id AND (i.IsDeleted IS NULL OR i.IsDeleted = 0)";

            var imageDict = new Dictionary<int, Image>();

            var images = await connection.QueryAsync<Image, Tag, Image>(
                sql,
                (image, tag) =>
                {
                    if (!imageDict.TryGetValue(image.Id, out var img))
                    {
                        img = image;
                        img.Tags = new List<Tag>();
                        imageDict.Add(img.Id, img);
                    }

                    if (tag != null && !img.Tags.Any(t => t.Id == tag.Id))
                    {
                        img.Tags.Add(tag);
                    }

                    return img;
                },
                new { Id = id },
                splitOn: "Id");

            return imageDict.Values.FirstOrDefault();
        }

        public async Task<Image?> UpdateAsync(Image model, int id)
        {
            using var connection = GetConnection();

            var sql = @"
            UPDATE Images
            SET Title = @Title,
                Url = @Url,
                ManuscriptId = @ManuscriptId,
                IsDeleted = @IsDeleted
            WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                model.Title,
                model.Url,
                model.ManuscriptId,
                model.IsDeleted,
                Id = id
            });

            if (rowsAffected == 0)
                return null;

            await connection.ExecuteAsync("DELETE FROM ImageTag WHERE ImageId = @Id", new { Id = id });

            if (model.Tags?.Any() == true)
            {
                foreach (var tag in model.Tags)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO ImageTag (ImageId, TagId) VALUES (@ImageId, @TagId);",
                        new { ImageId = id, TagId = tag.Id }
                    );
                }
            }

            model.Id = id;
            return model;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

}
