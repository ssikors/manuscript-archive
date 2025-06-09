
using ManuscriptApi.DapperDAL;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.Services
{
    public class TagService : DapperCrudService<Tag>
    {
        public TagService(ITagRepository repository, ILogger<DapperCrudService<Tag>> logger) : base(repository, logger)
        {
        }
    }
}
