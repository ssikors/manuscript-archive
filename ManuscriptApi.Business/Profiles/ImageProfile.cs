using AutoMapper;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDto>()
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.Tags.Select(t => t.Id)))
                .ReverseMap()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}
