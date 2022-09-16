using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.DataAccess.Repositories
{
    public interface IBaseRepository<TEntity>: IDisposable
        where TEntity: BaseEntity
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAsNoTracking();

        IQueryable<TEntity> GetAllByCondition(Expression<Func<TEntity, bool>> filter);

        TEntity GetById(string id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(string id);

        int SaveChanges();
    }
}
