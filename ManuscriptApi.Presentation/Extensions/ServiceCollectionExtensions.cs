using ManuscriptApi.Business.Services;
using ManuscriptApi.DapperDAL.Repositories;
using ManuscriptApi.DapperDAL;

namespace ManuscriptApi.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IManuscriptRepository, ManuscriptRepository>();
            services.AddScoped<IUserAuthRepository, UserAuthRepository>();

            return services;
        }

        public static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            services.AddScoped<ICrudService<Country>, CountryService>();
            services.AddScoped<ICrudService<Location>, LocationService>();
            services.AddScoped<ICrudService<Tag>, TagService>();
            services.AddScoped<ICrudService<User>, UserService>();
            services.AddScoped<ICrudService<Image>, ImageService>();            
            services.AddScoped<ICrudService<Manuscript>, ManuscriptService>();

            return services;
        }
    }
}
