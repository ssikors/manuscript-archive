using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Services
{
    public interface IImageService
    {
        Task<List<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(int id);
        Task<Image> CreateAsync(ImageDto imageDto);
        Task<bool> UpdateAsync(int id, ImageDto imageDto);
        Task<bool> DeleteAsync(int id);
    }
}
