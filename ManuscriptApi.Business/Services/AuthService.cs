
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.DapperDAL;
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ManuscriptApi.Business.Services
{
    public class JwtSettings
    {
        public required string Token { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }

    public static class UserRoles
    {
        public const string User = "User";
        public const string Moderator = "Moderator";
    }

    public class AuthService : IAuthService
    {
        private readonly string _token;
        private readonly string _issuer;
        private readonly string _audience;

        private readonly IUserRepository _userRepository;
        private readonly IUserAuthRepository _userAuthRepository;

        public AuthService(IOptions<JwtSettings> jwtOptions, IUserRepository userRepository, IUserAuthRepository userAuthRepository)
        {
            var jwtSettings = jwtOptions.Value;

            _token = jwtSettings.Token;
            _issuer = jwtSettings.Issuer;
            _audience = jwtSettings.Audience;

            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
        }

        public async Task<int?> RegisterAsync(UserRegisterDto dto)
        {
            if (await _userRepository.GetUserByEmailAsync(dto.Email) != null)
            {
                return null;
            }

            User? user = await _userRepository.CreateAsync(new User
            {
                Username = dto.Username,
                Email = dto.Email,
            });

            var userAuth = new UserAuth();
            var passwordHash = new PasswordHasher<UserAuth>().HashPassword(userAuth, dto.Password);

            userAuth.PasswordHash = passwordHash;
            userAuth.UserId = user.Id;

            var authId = await _userAuthRepository.InsertUserAuth(userAuth);

            return authId;
        }

        public async Task<string?> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            var userAuth = await _userAuthRepository.GetUserAuth(user.Id);

            if (userAuth == null)
            {
                return null;
            }

            if (new PasswordHasher<UserAuth>().VerifyHashedPassword(userAuth, userAuth.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return CreateToken(userAuth, user);
        }

        private string CreateToken(UserAuth userAuth, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, userAuth.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsModerator ? UserRoles.Moderator : UserRoles.User)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_token));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
