using AutoMapper;
using PictureFinder.Application.Dto;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Presentation.MapperProfiles
{
    public class ChannelPostProfile : Profile
    {
        public ChannelPostProfile()
        {
            CreateMap<ChannelPost, ChannelPostDto>();
        }
    }
}