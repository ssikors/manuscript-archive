
using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Business.Services
{
    public class LocationService : DapperCrudService<Location>
    {
        public LocationService(ILocationRepository repository) : base(repository)
        {
        }
    }
}
