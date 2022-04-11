using AutoMapper;
using PictureFinder.Application.Dto;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Presentation.MapperProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageDto>();
        }
    }
}