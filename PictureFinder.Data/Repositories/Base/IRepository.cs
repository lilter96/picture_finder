using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PictureFinder.Domain.Base;

namespace PictureFinder.Data.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        public Task<TEntity> GetByIdAsync(Guid id);

        public Task<IList<TEntity>> GetAllAsync();

        public Task<bool> DeleteByIdAsync(Guid id);
    }
}