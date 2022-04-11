using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PictureFinder.Data.Repositories.Base;
using PictureFinder.Domain.Base;

namespace PictureFinder.Data.Sql.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected BaseRepository(ApplicationDbContext dbContext)
        {
            Context = dbContext;
        }

        protected ApplicationDbContext Context { get; }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();

            return true;
        }
    }
}