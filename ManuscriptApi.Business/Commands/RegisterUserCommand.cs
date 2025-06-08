
using MediatR;

namespace ManuscriptApi.Business.Commands
{
    public record RegisterUserCommand(string Username, string Email, bool IsModerator, string Password) : IRequest<int>
    {
    }
}
