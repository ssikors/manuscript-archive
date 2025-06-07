
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.Domain.Models;

namespace ManuscriptApi.Business.Services
{
    public interface IAuthService
    {
        public Task<int?> RegisterAsync(UserRegisterDto dto);

        public Task<string?> LoginAsync(UserLoginDto dto);
    }
}
