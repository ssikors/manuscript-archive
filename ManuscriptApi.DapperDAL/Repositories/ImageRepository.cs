﻿using System.Data;
using Dapper;

namespace ManuscriptApi.DapperDAL
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDbConnection _connection;

        public ImageRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Image> CreateAsync(Image model)
        {
            var sql = @"
            INSERT INTO Images (Title, Url, ManuscriptId, IsDeleted)
            VALUES (@Title, @Url, @ManuscriptId, @IsDeleted);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, model);
            model.Id = newId;

            if (model.Tags?.Any() == true)
            {
                var tagParams = model.Tags.Select(tag => new
                {
                    ImageId = newId,
                    TagId = tag.Id
                });

                await _connection.ExecuteAsync(
                    "INSERT INTO ImageTag (ImageId, TagId) VALUES (@ImageId, @TagId);",
                    tagParams
                );
            }


            return model;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Images WHERE Id = @Id";
            var rowsAffected = await _connection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<List<Image>> GetAllAsync()
        {
            var sql = @"
        SELECT i.*, t.Id, t.Name, t.Description, t.IsDeleted
        FROM Images i
        LEFT JOIN ImageTag it ON i.Id = it.ImageId
        LEFT JOIN Tags t ON it.TagId = t.Id
        WHERE i.IsDeleted IS NULL OR i.IsDeleted = 0";

            var imageDict = new Dictionary<int, Image>();

            var images = await _connection.QueryAsync<Image, Tag, Image>(
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
            var sql = @"
        SELECT i.*, t.Id, t.Name, t.Description, t.IsDeleted
        FROM Images i
        LEFT JOIN ImageTag it ON i.Id = it.ImageId
        LEFT JOIN Tags t ON it.TagId = t.Id
        WHERE i.Id = @Id AND (i.IsDeleted IS NULL OR i.IsDeleted = 0)";

            var imageDict = new Dictionary<int, Image>();

            var images = await _connection.QueryAsync<Image, Tag, Image>(
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
            var sql = @"
        UPDATE Images
        SET Title = @Title,
            Url = @Url,
            ManuscriptId = @ManuscriptId,
            IsDeleted = @IsDeleted
        WHERE Id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                model.Title,
                model.Url,
                model.ManuscriptId,
                model.IsDeleted,
                Id = id
            });

            if (rowsAffected == 0)
                return null;

            await _connection.ExecuteAsync("DELETE FROM ImageTag WHERE ImageId = @Id", new { Id = id });

            if (model.Tags?.Any() == true)
            {
                var tagParams = model.Tags.Select(tag => new
                {
                    ImageId = id,
                    TagId = tag.Id
                });

                await _connection.ExecuteAsync(
                    "INSERT INTO ImageTag (ImageId, TagId) VALUES (@ImageId, @TagId);",
                    tagParams
                );
            }

            model.Id = id;
            return model;
        }
    }

}
