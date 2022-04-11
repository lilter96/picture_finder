﻿using AutoMapper;
using PictureFinder.Application.Dto;
using PictureFinder.Integration.Telegram.Models;

namespace PictureFinder.Presentation.MapperProfiles
{
    public class MessageEntityProfile : Profile
    {
        public MessageEntityProfile()
        {
            CreateMap<MessageEntity, MessageEntityDto>();
        }
    }
}