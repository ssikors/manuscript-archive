using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Business.Services
{
    public class ManuscriptService : DapperCrudService<Manuscript>
    {
        public ManuscriptService(IManuscriptRepository repository) : base(repository)
        {
        }
    }
}
