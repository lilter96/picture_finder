using AutoMapper;
using PictureFinder.Application.Dto;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Presentation.MapperProfiles
{
    public class PhotoSizeProfile : Profile
    {
        public PhotoSizeProfile()
        {
            CreateMap<PhotoSize, PhotoSizeDto>();
        }
    }
}