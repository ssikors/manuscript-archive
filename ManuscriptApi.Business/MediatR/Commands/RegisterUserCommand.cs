using MediatR;

namespace ManuscriptApi.Business.MediatR.Commands
{
    public record RegisterUserCommand(string Username, string Email, bool IsModerator, string Password) : IRequest<int>
    {
    }
}
