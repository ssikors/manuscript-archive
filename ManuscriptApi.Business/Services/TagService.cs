using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class TagService : ITagService
    {
        private readonly ManuscriptDbContext _context;

        public TagService(ManuscriptDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags
                .Where(t => t.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted != true);
        }

        public async Task<Tag> CreateAsync(TagDto tagDto)
        {
            var tag = new Tag
            {
                Name = tagDto.Name,
                Description = tagDto.Description,
                IsDeleted = false
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task<bool> UpdateAsync(int id, TagDto tagDto)
        {
            var existing = await _context.Tags.FindAsync(id);
            if (existing == null || existing.IsDeleted == true) return false;

            existing.Name = tagDto.Name;
            existing.Description = tagDto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null || tag.IsDeleted == true) return false;

            tag.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
