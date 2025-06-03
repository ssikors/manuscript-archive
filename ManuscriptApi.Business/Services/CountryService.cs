
using ManuscriptApi.Business.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.Business.Services
{
    public class CountryService : BaseDbCrudService<Country, ManuscriptDbContext>
    {
        public CountryService(ManuscriptDbContext context) : base(context)
        {
        }
    }

}
