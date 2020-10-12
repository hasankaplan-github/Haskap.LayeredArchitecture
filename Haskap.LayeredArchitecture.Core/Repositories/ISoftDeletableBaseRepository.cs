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
    public interface ISoftDeletableBaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> 
        where TEntity : BaseEntity<TId>
    {
        void SoftDelete(TId id);
        void SoftDelete(TEntity entity);
        void SoftDelete(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        void SoftDeleteRange(params TEntity[] entities);
        void SoftDeleteRange(IEnumerable<TEntity> entities);

        void UnDelete(TId id);
        void UnDelete(TEntity entity);
        void UnDelete(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        void UnDeleteRange(params TEntity[] entities);
        void UnDeleteRange(IEnumerable<TEntity> entities);


        TEntity GetSoftDeleted(Expression<Func<TEntity, bool>> where, string includeProperties = "");
        IList<TEntity> GetManySoftDeleted(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IList<TEntity> GetAllSoftDeleted(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetManySoftDeletedAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetManySoftDeletedAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IList<TEntity>> GetAllSoftDeletedAsync(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<PagedList<TEntity>> GetAllSoftDeletedAsync(int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    }
}
