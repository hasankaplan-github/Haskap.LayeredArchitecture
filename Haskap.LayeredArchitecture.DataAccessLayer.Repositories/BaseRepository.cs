using Haskap.LayeredArchitecture.Core.Entities;
using Haskap.LayeredArchitecture.Core.Repositories;
using Haskap.LayeredArchitecture.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.DataAccessLayer.Repositories
{
    public class BaseRepository<TEntity, TId, TDbContext> : IBaseRepository<TEntity, TId>
        where TEntity : BaseEntity<TId>
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected readonly TDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException("dbContext can not be null.");
            this.dbSet = dbContext.Set<TEntity>();
        }

        #region IRepository Members

        public virtual TEntity GetById(TId id)
        {
            return this.dbSet.Find(id);
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, string includeProperties = "", bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.SingleOrDefault();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            if (include != null)
            {
                query = include(query);
            }

            return query.SingleOrDefault();
        }

        public virtual IList<TEntity> GetAll(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual IList<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }


        public virtual IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }



        public virtual void Add(TEntity entity)
        {
            //entity.Id = Guid.NewGuid();
            this.dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            //if (this.dbContext.Entry(entity).State == EntityState.Detached)
            //{
            //    this.dbSet.Attach(entity);
            //    //this.dbContext.Entry(entity).Property(p => p.ClusteredIndex).IsModified = false;
            //}
            //this.dbSet.Attach(entity);
            //this.dbContext.Entry(entity).State = EntityState.Modified;
            this.dbSet.Update(entity);
        }

        public virtual void UpdateRange(params TEntity[] entities)
        {
            //if (this.dbContext.Entry(entity).State == EntityState.Detached)
            //{
            //    this.dbSet.Attach(entity);
            //    //this.dbContext.Entry(entity).Property(p => p.ClusteredIndex).IsModified = false;
            //}
            //this.dbSet.Attach(entity);
            //this.dbContext.Entry(entity).State = EntityState.Modified;
            this.dbSet.UpdateRange(entities);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            this.dbSet.UpdateRange(entities);
        }


        public virtual void Delete(TEntity entity)
        {
            //if (this.dbContext.Entry(entity).State == EntityState.Detached)
            //{
            //    this.dbSet.Attach(entity);
            //}
            this.dbSet.Remove(entity);
        }

        public virtual void Delete(TId id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                Delete(entity);
            }
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where, string includeProperties = "")
        {
            var entities = GetMany(where, includeProperties);
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var entities = GetMany(where, include);
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void DeleteRange(params TEntity[] entities)
        {
            this.dbSet.RemoveRange(entities);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            this.dbSet.RemoveRange(entities);
        }



        protected virtual async Task<PagedList<TEntity>> GetPagedListAsync(IQueryable<TEntity> source, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;
            var totalItemCount = await source.CountAsync();
            var pagedQuery = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var pagedItemsCount = await pagedQuery.CountAsync();
            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            if (pagedItemsCount == 0)
            {
                pageIndex = totalPageCount == 0 ? 1 : totalPageCount;
                pagedQuery = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            var pagedItems = await pagedQuery.ToListAsync();
            return new PagedList<TEntity>(pagedItems, totalItemCount, pageIndex, pageSize);
        }

        protected virtual PagedList<TEntity> GetPagedList(IList<TEntity> source, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;
            var totalItemCount = source.Count();
            var pagedItems = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var pagedItemsCount = pagedItems.Count();
            var totalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            if (pagedItemsCount == 0)
            {
                pageIndex = totalPageCount == 0 ? 1 : totalPageCount;
                pagedItems = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return new PagedList<TEntity>(pagedItems.ToList(), totalItemCount, pageIndex, pageSize);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = query.Where(e => (e as ISoftDeletable).IsDeleted == false);
            //}

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.Any(predicate);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = this.dbSet;
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = query.Where(e => (e as ISoftDeletable).IsDeleted == false);
            //}

            if (include != null)
            {
                query = include(query);
            }

            return query.Any(predicate);
        }

        public virtual IList<TEntity> FromSql(FormattableString sqlString)
        {
            return this.dbSet.FromSqlInterpolated(sqlString).ToList();
        }

        public virtual async Task<PagedList<TEntity>> FromSqlAsync(FormattableString sqlString, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            var source = this.dbSet.FromSqlInterpolated(sqlString);
            if (orderBy != null)
            {
                source = orderBy(source);
            }
            var pagedList = await this.GetPagedListAsync(source, pageIndex, pageSize);
            return pagedList;
        }

        public virtual async Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var pagedList = await this.GetPagedListAsync(query, pageIndex, pageSize);
            return pagedList;
        }

        public virtual async Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var pagedList = await this.GetPagedListAsync(query, pageIndex, pageSize);
            return pagedList;
        }

        public virtual async Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await this.GetPagedListAsync(query, pageIndex, pageSize);
        }

        public virtual async Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(where);

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await this.GetPagedListAsync(query, pageIndex, pageSize);
        }





        public virtual async Task<IList<TEntity>> GetAllAsync(string includeProperties = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }




        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = query.Where(e => (e as ISoftDeletable).IsDeleted == false);
            //}
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.CountAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = this.dbSet;
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = query.Where(e => (e as ISoftDeletable).IsDeleted == false);
            //}

            if (include != null)
            {
                query = include(query);
            }

            return await query.CountAsync(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate, string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = query.Where(e => (e as ISoftDeletable).IsDeleted == false);
            //}
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.Count(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = this.dbSet;
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            //{
            //    query = query.Where(e => (e as ISoftDeletable).IsDeleted == false);
            //}

            if (include != null)
            {
                query = include(query);
            }

            return query.Count(predicate);
        }
        #endregion
    }

}
