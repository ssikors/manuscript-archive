using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ManuscriptApi.Business.DTOs;

namespace ManuscriptApi.Business.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDto>().ReverseMap();
        }
    }
}
