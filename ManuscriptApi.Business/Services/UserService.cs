using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using ManuscriptApi.DataAccess.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class UserService : DapperCrudService<User>
    {
        public UserService(IUserRepository repository) : base(repository)
        {
        }
    }
}
