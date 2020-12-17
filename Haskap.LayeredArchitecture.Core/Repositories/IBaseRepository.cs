using Haskap.LayeredArchitecture.Core.Entities;
using Haskap.LayeredArchitecture.Core.Specifications;
using Haskap.LayeredArchitecture.Utilities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.Core.Repositories
{
    public interface IBaseRepository<TEntity, TId> 
        where TEntity : BaseEntity<TId>
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
        TEntity Get(Expression<Func<TEntity, bool>> where, string includeProperties = "", bool disableTracking = false);

        // Gets entities using delegate
        IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);

        //IQueryable<T> GetMany(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        IList<TEntity> GetAll(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);



        bool Exists(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");

        IList<TEntity> FromSql(FormattableString sqlString);
        Task<PagedList<TEntity>> FromSqlAsync(FormattableString sqlString, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<IList<TEntity>> GetAllAsync(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");
        int Count(Expression<Func<TEntity, bool>> predicate, string includeProperties = "");

        int Count(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        bool Exists(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        void Delete(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        IList<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        TEntity Get(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = false);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, string include = "", CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, CancellationToken cancellationToken = default);

        IList<TEntity> GetManyWhile(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        IList<TEntity> GetManyWhile(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<IList<TEntity>> GetManyWhileAsync(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
        Task<IList<TEntity>> GetManyWhileAsync(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false);
    }
}
