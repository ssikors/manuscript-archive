
using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Business.Services
{
    public class CountryService : DapperCrudService<Country>
    {
        public CountryService(ICountryRepository repository) : base(repository)
        {
        }
    }

}
