using System.Collections.Generic;
using System.Threading.Tasks;
using PictureFinder.Data.Repositories.Base;
using PictureFinder.Domain.Photo;
using PictureFinder.Domain.Photo.Dto;

namespace PictureFinder.Data.Repositories
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        public Task<List<PhotoWithTagsResponseDto>> GetPhotoByTags(List<string> tags);

        public Task<Photo> AddPhotoWithTags(AddPhotoWithTagsRequestDto addPhotoWithTagsRequestDto);
    }
}