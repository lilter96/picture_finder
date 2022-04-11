using System.Collections.Generic;
using System.Threading.Tasks;
using PictureFinder.Data.Repositories.Base;
using PictureFinder.Domain.Tag;

namespace PictureFinder.Data.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Task<List<Tag>> SetIdsIfExists(List<Tag> tags);
    }
}