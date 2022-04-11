using AutoMapper;
using PictureFinder.Application.Dto;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Presentation.MapperProfiles
{
    public class UpdateProfile : Profile
    {
        public UpdateProfile()
        {
            CreateMap<Update, UpdateDto>();
        }
    }
}