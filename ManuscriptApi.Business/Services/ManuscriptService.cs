using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class ManuscriptService : IManuscriptService
    {
        private readonly ManuscriptDbContext _context;

        public ManuscriptService(ManuscriptDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manuscript>> GetAllAsync()
        {
            return await _context.Manuscripts
                .Where(m => m.IsDeleted != true)
                .Include(m => m.Tags)
                .Include(m => m.Author)
                .Include(m => m.Location)
                .ToListAsync();
        }

        public async Task<Manuscript?> GetByIdAsync(int id)
        {
            return await _context.Manuscripts
                .Include(m => m.Tags)
                .Include(m => m.Author)
                .Include(m => m.Location)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted != true);
        }

        public async Task<Manuscript> CreateAsync(ManuscriptDto dto)
        {
            var manuscript = new Manuscript
            {
                Title = dto.Title,
                Description = dto.Description,
                YearWrittenStart = dto.YearWrittenStart,
                YearWrittenEnd = dto.YearWrittenEnd,
                SourceUrl = dto.SourceUrl,
                CreatedAt = DateTime.UtcNow,
                LocationId = dto.LocationId,
                AuthorId = dto.AuthorId,
            };

            if (dto.TagIds?.Any() == true)
            {
                manuscript.Tags = await _context.Tags
                    .Where(t => dto.TagIds.Contains(t.Id))
                    .ToListAsync();
            }

            _context.Manuscripts.Add(manuscript);
            await _context.SaveChangesAsync();
            return manuscript;
        }

        public async Task<bool> UpdateAsync(int id, ManuscriptDto dto)
        {
            var manuscript = await _context.Manuscripts
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manuscript == null)
                return false;

            manuscript.Title = dto.Title;
            manuscript.Description = dto.Description;
            manuscript.YearWrittenStart = dto.YearWrittenStart;
            manuscript.YearWrittenEnd = dto.YearWrittenEnd;
            manuscript.SourceUrl = dto.SourceUrl;
            manuscript.LocationId = dto.LocationId;
            manuscript.AuthorId = dto.AuthorId;

            if (dto.TagIds != null)
            {
                manuscript.Tags = await _context.Tags
                    .Where(t => dto.TagIds.Contains(t.Id))
                    .ToListAsync();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var manuscript = await _context.Manuscripts.FindAsync(id);
            if (manuscript == null) return false;

            manuscript.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
