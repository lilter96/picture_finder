using System.Collections.Generic;

namespace PictureFinder.Application.Dto
{
    public abstract class PhotosContainerDto
    {
        public string MediaGroupId { get; set; }

        public string Caption { get; set; }

        public List<MessageEntityDto> CaptionEntities { get; set; }

        public List<PhotoSizeDto> Photo { get; set; }
    }
}