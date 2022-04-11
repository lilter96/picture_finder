﻿using System.Collections.Generic;

namespace PictureFinder.Integration.Telegram.Models
{
    public class Message
    {
        public string Caption { get; set; }

        public List<MessageEntity> CaptionEntities { get; set; }
        
        public List<PhotoSize> Photo { get; set; }
    }
}