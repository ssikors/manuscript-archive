using ManuscriptApi.DapperDAL;
using Microsoft.Extensions.Logging;

namespace ManuscriptApi.Business.Services
{
    public class ImageService : DapperCrudService<Image>
    {
        public ImageService(IImageRepository repository, ILogger<DapperCrudService<Image>> logger) : base(repository, logger)
        {
        }
    }
}
