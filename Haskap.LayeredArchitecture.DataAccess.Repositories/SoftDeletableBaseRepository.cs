using Haskap.LayeredArchitecture.Core.Entities;
using Haskap.LayeredArchitecture.Core.Repositories;
using Haskap.LayeredArchitecture.Core.Specifications;
using Haskap.LayeredArchitecture.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.DataAccess.Repositories
{
    public class SoftDeletableBaseRepository<TEntity, TId, TDbContext> : BaseRepository<TEntity, TId, TDbContext>, ISoftDeletableBaseRepository<TEntity, TId>
       where TEntity : BaseEntity<TId>, ISoftDeletable
       where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SoftDeletableBaseRepository(TDbContext dbContext) : base(dbContext)
        {

        }

        protected override IQueryable<TEntity> PrepareQuery(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = base.PrepareQuery(where, orderBy, disableTracking);
            return query.IgnoreQueryFilters().Where(e => e.IsDeleted == true);
        }

        public virtual TEntity GetDeleted(Expression<Func<TEntity, bool>> where, string include = "", bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy: null, disableTracking);
            query = AddInclude(query, include);

            return query.SingleOrDefault();
        }

        public virtual IList<TEntity> GetAllDeleted(string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, disableTracking);
            query = AddInclude(query, include);
            return query.ToList();
        }

        public virtual IList<TEntity> GetManyDeleted(Expression<Func<TEntity, bool>> where, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, disableTracking);
            query = AddInclude(query, include);
            return query.ToList();
        }

        public virtual async Task<IList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await query.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await GetPagedListAsync(query, pageIndex, pageSize);
        }

        public virtual async Task<IList<TEntity>> GetAllDeletedAsync(string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await query.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetAllDeletedAsync(int pageIndex, int pageSize, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await GetPagedListAsync(query, pageIndex, pageSize);
        }



        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            //entity.DeletionDate = DateTime.Now;
            Update(entity);



            // Eğer sizlerde genelde bir kayıtı silmek yerine IsDelete şeklinde bool bir flag alanı tutuyorsanız,
            // Küçük bir refleciton kodu yardımı ile bunuda otomatikleştirebiliriz.
            //if (entity.GetType().GetProperty("IsDelete") != null)
            //{
            //    TEntity _entity = entity;

            //    _entity.GetType().GetProperty("IsDelete").SetValue(_entity, true);

            //    this.Update(_entity);
            //}
            //else
            //{
            // Önce entity'nin state'ini kontrol etmeliyiz.
            /*
            DbEntityEntry dbEntityEntry = dbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.dbSet.Attach(entity);
                this.dbSet.Remove(entity);
            }
            */
            //    this.dbSet.Attach(entity);
            //    this.dbSet.Remove(entity);
            //}
        }


        public override void Delete(TId id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                Delete(entity);
                /*
                if (entity.GetType().GetProperty("IsDelete") != null)
                {
                    TEntity _entity = entity;
                    _entity.GetType().GetProperty("IsDelete").SetValue(_entity, true);

                    this.Update(_entity);
                }
                else
                {
                    Delete(entity);
                }
                */
            }
        }

        public override void Delete(Expression<Func<TEntity, bool>> where, string includeProperties = "")
        {
            var entities = GetMany(where, includeProperties);
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public override void DeleteRange(params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public override void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void UnDelete(TEntity entity)
        {
            entity.IsDeleted = false;
            Update(entity);
        }

        public virtual void UnDelete(TId id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                UnDelete(entity);
            }
        }

        public virtual void UnDelete(Expression<Func<TEntity, bool>> where, string includeProperties = "")
        {
            var entities = GetManyDeleted(where, includeProperties);
            foreach (var entity in entities)
            {
                UnDelete(entity);
            }
        }

        public virtual void UnDeleteRange(params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                UnDelete(entity);
            }
        }

        public virtual void UnDeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                UnDelete(entity);
            }
        }

        public virtual void Drop(TEntity entity)
        {
            base.Delete(entity);
        }

        public virtual void Drop(TId id)
        {
            base.Delete(id);
        }

        public virtual void Drop(Expression<Func<TEntity, bool>> where, string includeProperties = "")
        {
            base.Delete(where, includeProperties);
        }

        public virtual void DropRange(params TEntity[] entities)
        {
            base.DeleteRange(entities);
        }

        public virtual void DropRange(IEnumerable<TEntity> entities)
        {
            base.DeleteRange(entities);
        }

        public virtual void Drop(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            base.Delete(where, include);
        }

        public virtual TEntity GetDeleted(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy: null, disableTracking);
            query = AddInclude(query, include);

            return query.SingleOrDefault();
        }

        public virtual IList<TEntity> GetManyDeleted(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy: null, disableTracking);
            query = AddInclude(query, include);
            return query.ToList();
        }

        public virtual IList<TEntity> GetAllDeleted(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, disableTracking);
            query = AddInclude(query, include);
            return query.ToList();
        }

        public virtual async Task<IList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await query.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetManyDeletedAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await GetPagedListAsync(query, pageIndex, pageSize);
        }

        public virtual async Task<IList<TEntity>> GetAllDeletedAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await query.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetAllDeletedAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, disableTracking);
            query = AddInclude(query, include);
            return await GetPagedListAsync(query, pageIndex, pageSize);
        }
    }
}
