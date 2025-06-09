using AutoMapper;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Profiles
{
    public class ManuscriptProfile : Profile
    {
        public ManuscriptProfile()
        {
            CreateMap<Manuscript, ManuscriptDto>()
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(t => t.Id)))
                .ReverseMap()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}
