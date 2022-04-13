using System.Collections.Generic;
using System.Threading.Tasks;
using PictureFinder.Data.Repositories.Base;
using PictureFinder.Domain.Photo;
using PictureFinder.Domain.Photo.Dto;
using PictureFinder.Domain.Tag;

namespace PictureFinder.Data.Repositories
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        public Task<List<PhotoWithTagsResponseDto>> GetPhotoByTagsAsync(List<string> tags);

        public Task<Photo> AddPhotoWithTagsAsync(AddPhotoWithTagsRequestDto addPhotoWithTagsRequestDto);

        public Task<List<Tag>> GetTagsFromSameMediaGroupsAsync(string messageId);
    }
}