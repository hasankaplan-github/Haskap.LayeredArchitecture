using Haskap.LayeredArchitecture.Core.Entities;
using Haskap.LayeredArchitecture.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.Core.Repositories
{
    public interface IBaseRepository<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        // Marks an entity as new
        void Add(TEntity entity);

        // Marks an entity as modified
        void Update(TEntity entity);
        void UpdateRange(params TEntity[] entities);
        void UpdateRange(IEnumerable<TEntity> entities);

        // Marks an entity to be removed
        void Delete(TEntity entity);
        void Delete(TId id);
        void Delete(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        void DeleteRange(params TEntity[] entities);
        void DeleteRange(IEnumerable<TEntity> entities);



        // Get an entity by int id
        TEntity GetById(TId id);
        // Get an entity using delegate
        TEntity Get(Expression<Func<TEntity, bool>> where, string includeProperties = "");

        // Gets entities using delegate
        IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        //IQueryable<T> GetMany(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        IList<TEntity> GetAll(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);



        bool Exists(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");

        IList<TEntity> FromSql(FormattableString sqlString);
        Task<PagedList<TEntity>> FromSqlAsync(FormattableString sqlString, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetAllAsync(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        int Count(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
    }
}
