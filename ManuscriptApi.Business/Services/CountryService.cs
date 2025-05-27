
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class CountryService : ICountryService
    {
        private readonly ManuscriptDbContext _context;

        public CountryService(ManuscriptDbContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task<Country> CreateAsync(CountryDto countryDto)
        {
            var country = new Country
            {
                Description = countryDto.Description,
                IconUrl = countryDto.IconUrl,
                Name = countryDto.Name,
            };

            _context.Countries.Add(country);

            await _context.SaveChangesAsync();

            return country;
        }

        public async Task<bool> UpdateAsync(int id, CountryDto countryDto)
        {
            var existing = await _context.Countries.FindAsync(id);
            if (existing == null) return false;

            existing.Name = countryDto.Name;
            existing.Description = countryDto.Description;
            existing.IconUrl = countryDto.IconUrl;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return false;

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
