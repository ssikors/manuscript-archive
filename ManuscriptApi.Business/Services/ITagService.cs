using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Services
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> CreateAsync(TagDto tagDto);
        Task<bool> UpdateAsync(int id, TagDto tagDto);
        Task<bool> DeleteAsync(int id);
    }
}
