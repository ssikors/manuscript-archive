using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class LocationService : ILocationService
    {
        private readonly ManuscriptDbContext _context;

        public LocationService(ManuscriptDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations
                .Include(l => l.Country)
                .Include(l => l.Manuscripts)
                .ToListAsync();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations
                .Include(l => l.Country)
                .Include(l => l.Manuscripts)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Location> CreateAsync(LocationDto locationDto)
        {
            var location = new Location
            {
                Name = locationDto.Name,
                CountryId = locationDto.CountryId
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<bool> UpdateAsync(int id, LocationDto locationDto)
        {
            var existing = await _context.Locations.FindAsync(id);
            if (existing == null) return false;

            existing.Name = locationDto.Name;
            existing.CountryId = locationDto.CountryId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return false;

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
