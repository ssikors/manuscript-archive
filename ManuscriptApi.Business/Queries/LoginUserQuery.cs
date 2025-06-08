using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ManuscriptApi.Business.Queries
{
    public record LoginUserQuery(string Email, string Password) : IRequest<string?>
    {
    }
}
