using System.Collections.Generic;
using System.Threading.Tasks;
using PictureFinder.Domain.Photo.Dto;

namespace PictureFinder.Application.WebServices
{
    public interface IPhotoService
    {
        public Task<List<PhotoWithTagsResponseDto>> GetPhotosWithTagsByTagNameAsync(string tagName);

        public Task<bool> DeletePhotosByTagNameAsync(string tagName);
    }
}