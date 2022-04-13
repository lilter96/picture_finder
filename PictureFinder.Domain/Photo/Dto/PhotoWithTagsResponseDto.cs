using System;
using System.Collections.Generic;

namespace PictureFinder.Domain.Photo.Dto
{
    public class PhotoWithTagsResponseDto
    {
        public Guid PhotoId { get; set; }

        public string ImageUrl { get; set; }

        public List<Tag.Tag> Tags { get; set; }
    }
}