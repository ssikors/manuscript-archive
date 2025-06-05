
using ManuscriptApi.DataAccess.Data.Repositories;

namespace ManuscriptApi.Business.Services
{
    public class CountryService : DapperCrudService<Country>
    {
        public CountryService(ICountryRepository repository) : base(repository)
        {
        }
    }

}
