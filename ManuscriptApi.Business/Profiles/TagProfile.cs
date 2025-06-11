using AutoMapper;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>().ReverseMap();
        }
    }
}
