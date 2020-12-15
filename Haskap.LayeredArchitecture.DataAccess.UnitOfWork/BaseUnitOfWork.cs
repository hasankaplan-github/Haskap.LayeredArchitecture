using Haskap.LayeredArchitecture.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.DataAccess.UnitOfWork
{
    public class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork
      where TDbContext : DbContext
    {
        protected readonly TDbContext DbContext;

        public BaseUnitOfWork(TDbContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException("dbContext can not be null.");
        }


        #region IBaseUnitOfWork Members

        public virtual IDbContextTransaction CurrentTransaction 
        { 
            get
            {
                return this.DbContext.Database.CurrentTransaction;
            }
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            return this.DbContext.Database.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            this.DbContext.Database.CommitTransaction();
        }

        public virtual void RollbackTransaction()
        {
            this.DbContext.Database.RollbackTransaction();
        }

        public virtual int SaveChanges()
        {
            try
            {
                int retVal = this.DbContext.SaveChanges();
                return retVal;
            }
            catch (Exception ex)
            {
                //TODO:Logging
                throw;
            }
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var retVal = await this.DbContext.SaveChangesAsync(cancellationToken);
                return retVal;
            }
            catch (Exception ex)
            {
                //TODO:Logging
                throw;
            }
        }
        #endregion


        #region IDisposable Members
        // Burada IUnitOfWork arayüzüne implemente ettiğimiz IDisposable arayüzünün Dispose Patternini implemente ediyoruz.
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
