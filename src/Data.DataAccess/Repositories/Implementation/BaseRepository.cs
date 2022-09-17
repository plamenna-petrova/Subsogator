﻿using Data.DataModels.Abstraction;
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

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"The object of type {typeof(TEntity)} " +
                    $"with id: {entity.Id} does not exist");
            }

            DbSet.Update(entity);
        }

        public virtual void Delete(string id)
        {
            var entityToDelete = GetById(id);

            if (entityToDelete == null)
            {
                throw new ArgumentNullException($"The object of type {typeof(TEntity)} " +
                    $"with id: {id} does not exist");
            }

            DbSet.Remove(entityToDelete);
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