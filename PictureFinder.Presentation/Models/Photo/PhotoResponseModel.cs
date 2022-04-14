using System.Collections.Generic;
using PictureFinder.Domain.Photo.Dto;

namespace PictureFinder.Presentation.Models.Photo
{
    public class PhotoResponseModel
    {
        public List<PhotoWithTagsResponseDto> Photos { get; set; }

        public string SearchTagName { get; set; }
    }
}