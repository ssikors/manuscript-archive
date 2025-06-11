
using ManuscriptApi.DapperDAL;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.Services
{
    public class LocationService : DapperCrudService<Location>
    {
        public LocationService(ILocationRepository repository, ILogger<DapperCrudService<Location>> logger) : base(repository, logger)
        {
        }
    }
}
