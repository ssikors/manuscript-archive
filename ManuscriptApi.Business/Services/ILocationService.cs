using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Services
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task<Location> CreateAsync(LocationDto locationDto);
        Task<bool> UpdateAsync(int id, LocationDto locationDto);
        Task<bool> DeleteAsync(int id);
    }
}
