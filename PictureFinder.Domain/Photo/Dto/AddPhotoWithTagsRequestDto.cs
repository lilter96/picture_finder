using System.Collections.Generic;

namespace PictureFinder.Domain.Photo.Dto
{
    public class AddPhotoWithTagsRequestDto
    {
        public string MediaGroupId { get; set; }

        public string PhotoUrl { get; set; }

        public List<Tag.Tag> Tags { get; set; }
    }
}