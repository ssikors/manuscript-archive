using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class ImageService : IImageService
    {
        private readonly ManuscriptDbContext _context;

        public ImageService(ManuscriptDbContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAllAsync()
        {
            return await _context.Images
                .Include(i => i.Tags)
                .Include(i => i.Manuscript)
                .Where(i => i.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images
                .Include(i => i.Tags)
                .Include(i => i.Manuscript)
                .FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted != true);
        }

        public async Task<Image> CreateAsync(ImageDto imageDto)
        {
            var tags = await _context.Tags
                .Where(t => imageDto.TagIds.Contains(t.Id) && t.IsDeleted != true)
                .ToListAsync();

            var image = new Image
            {
                Title = imageDto.Title,
                Url = imageDto.Url,
                ManuscriptId = imageDto.ManuscriptId,
                Tags = tags
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<bool> UpdateAsync(int id, ImageDto imageDto)
        {
            var image = await _context.Images
                .Include(i => i.Tags)
                .FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted != true);

            if (image == null) return false;

            image.Title = imageDto.Title;
            image.Url = imageDto.Url;
            image.ManuscriptId = imageDto.ManuscriptId;

            var tags = await _context.Tags
                .Where(t => imageDto.TagIds.Contains(t.Id) && t.IsDeleted != true)
                .ToListAsync();

            image.Tags = tags;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null || image.IsDeleted == true) return false;

            image.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
