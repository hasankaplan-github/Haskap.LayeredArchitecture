﻿using Haskap.LayeredArchitecture.Core.Entities;
using Haskap.LayeredArchitecture.Core.Repositories;
using Haskap.LayeredArchitecture.Core.Specifications;
using Haskap.LayeredArchitecture.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.DataAccess.Repositories
{
    public class BaseRepository<TEntity, TId, TDbContext> : IBaseRepository<TEntity, TId>
        where TEntity : BaseEntity<TId>
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected readonly TDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(TDbContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException("dbContext can not be null.");
            this.DbSet = dbContext.Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> PrepareQuery(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Expression<Func<TEntity, bool>> whilePredicate = null, bool disableTracking = false)
        {
            IQueryable<TEntity> query = this.DbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (whilePredicate != null)
            {
                query = query.TakeWhile(whilePredicate);
            }

            return query;
        }

        protected IQueryable<TEntity> AddInclude(IQueryable<TEntity> query, string include)
        {
            foreach (var includeProperty in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        protected IQueryable<TEntity> AddInclude(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            if (include != null)
            {
                query = include(query);
            }

            return query;
        }

        #region IRepository Members

        public virtual TEntity GetById(TId id)
        {
            return this.DbSet.IgnoreQueryFilters().SingleOrDefault(x => x.Id.Equals(id));
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, string include = "", bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy: null, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return query.SingleOrDefault();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy: null, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return query.SingleOrDefault();
        }

        public virtual IList<TEntity> GetAll(string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return query.ToList();
        }

        public virtual IList<TEntity> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return query.ToList();
        }


        public virtual IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return query.ToList();
        }

        public virtual IList<TEntity> GetMany(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return query.ToList();
        }



        public virtual void Add(TEntity entity)
        {
            //entity.Id = Guid.NewGuid();
            this.DbSet.Add(entity);
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
            this.DbSet.Update(entity);
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
            this.DbSet.UpdateRange(entities);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            this.DbSet.UpdateRange(entities);
        }


        public virtual void Delete(TEntity entity)
        {
            //if (this.dbContext.Entry(entity).State == EntityState.Detached)
            //{
            //    this.dbSet.Attach(entity);
            //}
            this.DbSet.Remove(entity);
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

        public virtual void Delete(Expression<Func<TEntity, bool>> where, string include = "")
        {
            var entities = GetMany(where, include);
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
            this.DbSet.RemoveRange(entities);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            this.DbSet.RemoveRange(entities);
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

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate, string include = "")
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return query.Any(predicate);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return query.Any(predicate);
        }

        public virtual IList<TEntity> FromSql(FormattableString sqlString)
        {
            return this.DbSet.FromSqlInterpolated(sqlString).ToList();
        }

        public virtual async Task<PagedList<TEntity>> FromSqlAsync(FormattableString sqlString, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            var source = this.DbSet.FromSqlInterpolated(sqlString);
            if (orderBy != null)
            {
                source = orderBy(source);
            }
            var pagedList = await this.GetPagedListAsync(source, pageIndex, pageSize);
            return pagedList;
        }

        public virtual async Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);
            var pagedList = await this.GetPagedListAsync(query, pageIndex, pageSize);
            return pagedList;
        }

        public virtual async Task<PagedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            var pagedList = await this.GetPagedListAsync(query, pageIndex, pageSize);
            return pagedList;
        }

        public virtual async Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return await query.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return await query.ToListAsync();
        }

        public virtual async Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return await this.GetPagedListAsync(query, pageIndex, pageSize);
        }

        public virtual async Task<PagedList<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return await this.GetPagedListAsync(query, pageIndex, pageSize);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return await query.ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where: null, orderBy, whilePredicate: null, disableTracking);
            query = AddInclude(query, include);

            return await query.ToListAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, string include = "")
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return await query.CountAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return await query.CountAsync(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate, string include = "")
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return query.Count(predicate);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return query.Count(predicate);
        }

        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            this.DbSet.AddAsync(entity, cancellationToken);
            return Task.CompletedTask;
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, string include = "", CancellationToken cancellationToken = default)
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return query.AnyAsync(predicate, cancellationToken);
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, CancellationToken cancellationToken = default)
        {
            var query = PrepareQuery(where: null, orderBy: null, whilePredicate: null, disableTracking: false);
            query = AddInclude(query, include);

            return query.AnyAsync(predicate, cancellationToken);
        }




        public virtual IList<TEntity> GetManyWhile(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate, disableTracking);
            query = AddInclude(query, include);

            return query.ToList();
        }

        public virtual IList<TEntity> GetManyWhile(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate, disableTracking);
            query = AddInclude(query, include);

            return query.ToList();
        }

        public async virtual Task<IList<TEntity>> GetManyWhileAsync(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, string include = "", Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate, disableTracking);
            query = AddInclude(query, include);

            return await query.ToListAsync();
        }

        public async virtual Task<IList<TEntity>> GetManyWhileAsync(Expression<Func<TEntity, bool>> whilePredicate, Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool disableTracking = false)
        {
            var query = PrepareQuery(where, orderBy, whilePredicate, disableTracking);
            query = AddInclude(query, include);

            return await query.ToListAsync();
        }


        #endregion
    }

}
