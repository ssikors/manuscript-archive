
using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Business.Services
{
    public class TagService : DapperCrudService<Tag>
    {
        public TagService(ITagRepository repository) : base(repository)
        {
        }
    }
}
