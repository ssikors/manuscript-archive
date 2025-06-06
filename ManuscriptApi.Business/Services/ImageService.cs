using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Business.Services
{
    public class ImageService : DapperCrudService<Image>
    {
        public ImageService(IImageRepository repository) : base(repository)
        {
        }
    }
}
