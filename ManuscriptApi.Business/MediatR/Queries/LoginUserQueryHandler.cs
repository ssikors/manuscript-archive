using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.DapperDAL;
using MediatR;
using ManuscriptApi.Business.Services;
using Microsoft.Extensions.Options;
using ManuscriptApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ManuscriptApi.Business.MediatR.Queries
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

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string?>
    {
        private readonly string _token;
        private readonly string _issuer;
        private readonly string _audience;

        private readonly IUserRepository _userRepository;
        private readonly IUserAuthRepository _userAuthRepository;

        public LoginUserQueryHandler(IOptions<JwtSettings> jwtOptions, IUserRepository userRepository, IUserAuthRepository userAuthRepository)
        {
            var jwtSettings = jwtOptions.Value;

            _token = jwtSettings.Token;
            _issuer = jwtSettings.Issuer;
            _audience = jwtSettings.Audience;

            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
        }

        public async Task<string?> Handle(LoginUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(query.Email);

            if (user == null)
            {
                return null;
            }

            var userAuth = await _userAuthRepository.GetUserAuth(user.Id);

            if (userAuth == null)
            {
                return null;
            }

            if (new PasswordHasher<UserAuth>().VerifyHashedPassword(userAuth, userAuth.PasswordHash, query.Password) == PasswordVerificationResult.Failed)
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
