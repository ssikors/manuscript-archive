using ManuscriptApi.Business.MediatR.Commands;
using ManuscriptApi.Business.MediatR.Queries;
using ManuscriptApi.DapperDAL;
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.Domain.Models;
using Moq;

namespace ManuscriptApi.Tests
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserAuthRepository> _userAuthRepositoryMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userAuthRepositoryMock = new Mock<IUserAuthRepository>();
            _handler = new RegisterUserCommandHandler(
                _userRepositoryMock.Object,
                _userAuthRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturn0_WhenUserAlreadyExists()
        {
            // Arrange
            var request = new RegisterUserCommand(Username: "user", Email: "user@test.com", Password: "password", IsModerator: false);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(new User { Email = request.Email });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new RegisterUserCommand(Username: "user", Email: "user@test.com", Password: "password", IsModerator: false );
         

            _userRepositoryMock
                .Setup(repo => repo.GetUserByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(new User { Id = 1, Email = request.Email });

            _userAuthRepositoryMock
                .Setup(repo => repo.InsertUserAuth(It.IsAny<UserAuth>()))
                .ReturnsAsync(42);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(42, result);
            _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
            _userAuthRepositoryMock.Verify(r => r.InsertUserAuth(It.Is<UserAuth>(a => a.UserId == 1)), Times.Once);
        }
    }
}