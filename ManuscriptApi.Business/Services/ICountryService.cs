

using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Services
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(int id);
        Task<Country> CreateAsync(CountryDto country);
        Task<bool> UpdateAsync(int id, CountryDto country);
        Task<bool> DeleteAsync(int id);
    }
}
