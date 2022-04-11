using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PictureFinder.Data.Repositories;
using PictureFinder.Domain.Tag;

namespace PictureFinder.Data.Sql.Repository
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected DbSet<Tag> Tags => Context.Tags;

        public async Task<List<Tag>> SetIdsIfExists(List<Tag> tags)
        {
            var result = new List<Tag>();

            foreach (var tag in tags)
            {
                var foundTag = await Tags.FirstOrDefaultAsync(t => t.Name == tag.Name);

                result.Add(foundTag ?? tag);
            }

            return result;
        }
    }
}