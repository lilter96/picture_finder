using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PictureFinder.Data.Repositories;
using PictureFinder.Domain.Photo;
using PictureFinder.Domain.Photo.Dto;
using PictureFinder.Domain.Tag;

namespace PictureFinder.Data.Sql.Repository
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public DbSet<Photo> Photos => Context.Photos;

        public async Task<List<PhotoWithTagsResponseDto>> GetPhotoByTagsAsync(List<string> tags)
        {
            return await Photos.Include(x => x.Tags).Where(photo => photo.Tags.Any(tag => tags.Any(y => tag.Name == y))).Select(
                photo => new PhotoWithTagsResponseDto
                {
                    PhotoId = photo.Id,
                    ImageUrl = photo.Url,
                    Tags = photo.Tags
                }).ToListAsync();
        }

        public async Task<Photo> AddPhotoWithTagsAsync(AddPhotoWithTagsRequestDto addPhotoWithTagsRequestDto)
        {
            var photo = new Photo
            {
                MediaGroupId = addPhotoWithTagsRequestDto.MediaGroupId,
                Url = addPhotoWithTagsRequestDto.PhotoUrl,
                Tags = addPhotoWithTagsRequestDto.Tags
            };

            var entity = await Photos.AddAsync(photo);
            await Context.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task<List<Tag>> GetTagsFromSameMediaGroupsAsync(string mediaGroupId)
        {
            var photo = await Photos.Include(p => p.Tags).FirstOrDefaultAsync(photo => photo.MediaGroupId == mediaGroupId);

            return photo?.Tags;
        }
    }
}