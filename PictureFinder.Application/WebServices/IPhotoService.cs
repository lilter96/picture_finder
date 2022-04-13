using System.Collections.Generic;
using System.Threading.Tasks;
using PictureFinder.Domain.Photo;
using PictureFinder.Domain.Photo.Dto;

namespace PictureFinder.Application.WebServices
{
    public interface IPhotoService
    {
        public Task<List<PhotoWithTagsResponseDto>> GetPhotosWithTagsByTagName(string tagName);
    }
}