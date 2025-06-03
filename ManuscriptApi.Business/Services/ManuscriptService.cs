using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class ManuscriptService : BaseDbCrudService<Manuscript, ManuscriptDbContext>
    {
        public ManuscriptService(ManuscriptDbContext context) : base(context)
        {
        }
    }
}
