
using System.IdentityModel.Tokens.Jwt;
using ManuscriptApi.Business.MediatR.Queries;
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.DapperDAL;
using ManuscriptApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Tests
{
    public class LoginUserQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserAuthRepository> _userAuthRepositoryMock;
        private readonly Mock<ILogger<LoginUserQueryHandler>> _loggerMock;
        private readonly LoginUserQueryHandler _handler;

        public LoginUserQueryHandlerTests()
        {
            var jwtSettings = new JwtSettings
            {
                Token = "MegaExtraSuperLongAndSuperSecretKeyForTestingTheLoginUserQueryHandler",
                Issuer = "TestIssuer",
                Audience = "TestAudience"
            };

            var jwtOptions = Options.Create(jwtSettings);

            _userRepositoryMock = new Mock<IUserRepository>();
            _userAuthRepositoryMock = new Mock<IUserAuthRepository>();
            _loggerMock = new Mock<ILogger<LoginUserQueryHandler>>();

            _handler = new LoginUserQueryHandler(
                jwtOptions,
                _userRepositoryMock.Object,
                _userAuthRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var query = new LoginUserQuery("nonexistent@example.com", "password");

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(query.Email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var query = new LoginUserQuery("user@example.com", "wrongpassword");

            var user = new User { Id = 1, Email = query.Email, IsModerator = false };
            var userAuth = new UserAuth
            {
                Id = 1,
                UserId = 1,
                PasswordHash = new PasswordHasher<UserAuth>().HashPassword(new UserAuth(), "correctpassword")
            };

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(query.Email)).ReturnsAsync(user);
            _userAuthRepositoryMock.Setup(r => r.GetUserAuth(user.Id)).ReturnsAsync(userAuth);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var query = new LoginUserQuery("user@example.com", "correctpassword");

            var user = new User { Id = 1, Email = query.Email, IsModerator = true };
            var userAuth = new UserAuth
            {
                Id = 2,
                UserId = 1,
            };

            userAuth.PasswordHash = new PasswordHasher<UserAuth>().HashPassword(userAuth, "correctpassword");

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(query.Email)).ReturnsAsync(user);
            _userAuthRepositoryMock.Setup(r => r.GetUserAuth(user.Id)).ReturnsAsync(userAuth);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result!);
            Assert.Equal("TestIssuer", token.Issuer);
            Assert.Equal("TestAudience", token.Audiences.First());
        }
    }
}
