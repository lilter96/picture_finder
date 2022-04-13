using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PictureFinder.Data.Repositories;
using PictureFinder.Domain.Photo;
using PictureFinder.Domain.Photo.Dto;

namespace PictureFinder.Application.WebServices
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoService(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<List<PhotoWithTagsResponseDto>> GetPhotosWithTagsByTagName(string tagName)
        {
            var dto = await _photoRepository.GetPhotoByTagsAsync(new List<string> {tagName});

            return dto;
        }
    }
}