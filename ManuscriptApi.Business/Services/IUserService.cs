using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(UserDto userDto);
        Task<bool> UpdateAsync(int id, UserDto userDto);
        Task<bool> DeleteAsync(int id);
    }
}
