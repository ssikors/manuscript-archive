
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.DapperDAL;
using MediatR;
using ManuscriptApi.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace ManuscriptApi.Business.MediatR.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAuthRepository _userAuthRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUserAuthRepository userAuthRepository)
        {
            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            {
                return 0;
            }

            User? user = await _userRepository.CreateAsync(new User
            {
                Username = request.Username,
                Email = request.Email,
                IsModerator = request.IsModerator,
            });

            var userAuth = new UserAuth();
            var passwordHash = new PasswordHasher<UserAuth>().HashPassword(userAuth, request.Password);

            userAuth.PasswordHash = passwordHash;
            userAuth.UserId = user.Id;

            var authId = await _userAuthRepository.InsertUserAuth(userAuth);

            return authId;
        }
    }
}
