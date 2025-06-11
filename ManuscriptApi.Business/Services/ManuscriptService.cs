using ManuscriptApi.DapperDAL;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.Services
{
    public class ManuscriptService : DapperCrudService<Manuscript>
    {
        public ManuscriptService(IManuscriptRepository repository, ILogger<DapperCrudService<Manuscript>> logger) : base(repository, logger)
        {
        }
    }
}
