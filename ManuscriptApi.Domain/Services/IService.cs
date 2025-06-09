
namespace ManuscriptApi.Domain.Services
{
   public interface IService<T> : IService where T : IModel
    {

    }

    public interface IService
    {

    }
}
