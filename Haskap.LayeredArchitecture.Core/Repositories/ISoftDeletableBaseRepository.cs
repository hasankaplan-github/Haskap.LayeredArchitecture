using Haskap.LayeredArchitecture.Core.Entities;
using Haskap.LayeredArchitecture.Core.Specifications;
using Haskap.LayeredArchitecture.Utilities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.Core.Repositories
{
    public interface ISoftDeletableBaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> 
        where TEntity : BaseEntity<TId>
    {
        void Drop(TId id);
        void Drop(TEntity entity);
        void Drop(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        void DropRange(params TEntity[] entities);
        void DropRange(IEnumerable<TEntity> entities);

        void UnDelete(TId id);
        void UnDelete(TEntity entity);
        void UnDelete(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        void UnDeleteRange(params TEntity[] entities);
        void UnDeleteRange(IEnumerable<TEntity> entities);


        TEntity GetDeleted(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        IList<TEntity> GetManyDeleted(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IList<TEntity> GetAllDeleted(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetAllDeletedAsync(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetAllDeletedAsync(int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);


        void Drop(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        TEntity GetDeleted(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        IList<TEntity> GetManyDeleted(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IList<TEntity> GetAllDeleted(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetAllDeletedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetAllDeletedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    }
}
