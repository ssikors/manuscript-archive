
using ManuscriptApi.DapperDAL;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace ManuscriptApi.Business.Services
{
    public class CountryService : DapperCrudService<Country>
    {
        public CountryService(ICountryRepository repository, ILogger<DapperCrudService<Country>> logger) : base(repository, logger)
        {
        }
    }

}
