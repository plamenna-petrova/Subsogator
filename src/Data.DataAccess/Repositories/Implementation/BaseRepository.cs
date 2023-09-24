using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Data.DataAccess.Repositories.Implementation
{
    public abstract class BaseRepository<TEntity>: IBaseRepository<TEntity>
        where TEntity: class
    {
        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext
               ?? throw new ArgumentNullException(nameof(applicationDbContext));
            DbSet = ApplicationDbContext.Set<TEntity>();
        }

        protected ApplicationDbContext ApplicationDbContext { get; set; }

        protected DbSet<TEntity> DbSet { get; set; }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual IQueryable<TEntity> GetAllAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        public virtual IQueryable<TEntity> GetAllByCondition(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Where(filter);
        }

        public virtual TEntity GetById(string id)
        {
            return DbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void AddRange(TEntity[] entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(TEntity[] entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void DeleteRange(TEntity[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual bool Exists(IQueryable<TEntity> entities, TEntity entityToFind)
        {
            return entities.Any(e => e == entityToFind);
        }

        public void Dispose()
        {
            DisposeApplicationDbContext(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeApplicationDbContext(bool disposing)
        {
            if (disposing)
            {
                ApplicationDbContext?.Dispose();
            }
        }
    }
}
