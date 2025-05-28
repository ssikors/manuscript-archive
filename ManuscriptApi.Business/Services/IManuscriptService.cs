using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Services
{
    public interface IManuscriptService
    {
        Task<List<Manuscript>> GetAllAsync();
        Task<Manuscript?> GetByIdAsync(int id);
        Task<Manuscript> CreateAsync(ManuscriptDto dto);
        Task<bool> UpdateAsync(int id, ManuscriptDto dto);
        Task<bool> DeleteAsync(int id);
    }

}
