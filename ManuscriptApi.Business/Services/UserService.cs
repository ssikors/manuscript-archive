using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ManuscriptDbContext _context;

        public UserService(ManuscriptDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted != true);
        }

        public async Task<User> CreateAsync(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                IsModerator = userDto.IsModerator,
                IsDeleted = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UpdateAsync(int id, UserDto userDto)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null || existing.IsDeleted == true)
                return false;

            existing.Username = userDto.Username;
            existing.Email = userDto.Email;
            existing.IsModerator = userDto.IsModerator;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.IsDeleted == true)
                return false;

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
