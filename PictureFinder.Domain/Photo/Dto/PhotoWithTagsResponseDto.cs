using System.Collections.Generic;

namespace PictureFinder.Domain.Photo.Dto
{
    public class PhotoWithTagsResponseDto
    {
        public string ImageUrl { get; set; }

        public List<Tag.Tag> Tags { get; set; }
    }
}