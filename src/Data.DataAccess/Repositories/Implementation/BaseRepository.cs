using Data.DataModels.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories.Implementation
{
    public abstract class BaseRepository<TEntity>: IBaseRepository<TEntity>
        where TEntity: BaseEntity
    {
        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            this.ApplicationDbContext = applicationDbContext
                    ?? throw new ArgumentNullException(nameof(applicationDbContext));
            this.DbSet = this.ApplicationDbContext.Set<TEntity>();
        }

        protected ApplicationDbContext ApplicationDbContext { get; set; }

        protected DbSet<TEntity> DbSet { get; set; }

        public virtual IQueryable<TEntity> GetAll()
        {
            return this.DbSet;
        }

        public virtual IQueryable<TEntity> GetAllAsNoTracking()
        {
            return this.DbSet.AsNoTracking();
        }

        public virtual IQueryable<TEntity> GetAllByCondition(Expression<Func<TEntity, bool>> filter)
        {
            return this.DbSet.Where(filter);
        }

        public virtual TEntity GetById(string id)
        {
            return this.DbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            var entityToUpdate = this.GetById(entity.Id);

            if (entityToUpdate == null)
            {
                throw new ArgumentNullException($"The object of type {typeof(TEntity)} " +
                    $"with id: {entity.Id} does not exist");
            }

            this.DbSet.Update(entity);
        }

        public void Delete(string id)
        {
            var entityToDelete = this.GetById(id);

            if (entityToDelete == null)
            {
                throw new ArgumentNullException($"The object of type {typeof(TEntity)} " +
                    $"with id: {id} does not exist");
            }

            this.DbSet.Remove(entityToDelete);
        }

        public int SaveChanges()
        {
            return this.ApplicationDbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.DisposeApplicationDbContext(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeApplicationDbContext(bool disposing)
        {
            if (disposing)
            {
                this.ApplicationDbContext?.Dispose();
            }
        }
    }
}
