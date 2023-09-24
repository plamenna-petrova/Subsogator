using System;
using System.Linq;
using System.Linq.Expressions;

namespace Data.DataAccess.Repositories
{
    public interface IBaseRepository<TEntity>: IDisposable where TEntity: class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAsNoTracking();

        IQueryable<TEntity> GetAllByCondition(Expression<Func<TEntity, bool>> filter);

        TEntity GetById(string id);

        void Add(TEntity entity);

        void AddRange(TEntity[] entities);

        void Update(TEntity entity);

        void UpdateRange(TEntity[] entities);

        void Delete(TEntity entity);

        void DeleteRange(TEntity[] entities);

        bool Exists(IQueryable<TEntity> entities, TEntity entityToFind);
    }
}
