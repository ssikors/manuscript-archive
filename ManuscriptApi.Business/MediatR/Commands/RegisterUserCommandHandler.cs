
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.DapperDAL;
using MediatR;
using ManuscriptApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.MediatR.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAuthRepository _userAuthRepository;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IUserAuthRepository userAuthRepository,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
            _logger = logger;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering user: {Email}", request.Email);

            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            {
                _logger.LogInformation("User already exists with email: {Email}", request.Email);
                return 0;
            }

            var user = await _userRepository.CreateAsync(new User
            {
                Username = request.Username,
                Email = request.Email,
                IsModerator = request.IsModerator,
            });

            _logger.LogInformation("Created user with ID: {UserId}", user.Id);

            var userAuth = new UserAuth();
            var passwordHash = new PasswordHasher<UserAuth>().HashPassword(userAuth, request.Password);

            userAuth.PasswordHash = passwordHash;
            userAuth.UserId = user.Id;

            var authId = await _userAuthRepository.InsertUserAuth(userAuth);

            _logger.LogInformation("Created auth entry with ID: {AuthId} for user: {UserId}", authId, user.Id);

            return authId;
        }
    }

}
