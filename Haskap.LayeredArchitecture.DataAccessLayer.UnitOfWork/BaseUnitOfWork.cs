using Haskap.LayeredArchitecture.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.LayeredArchitecture.DataAccessLayer.UnitOfWork
{
    public class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork
      where TDbContext : DbContext
    {
        protected readonly TDbContext dbContext;

        public BaseUnitOfWork(TDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException("dbContext can not be null.");
        }


        #region IBaseUnitOfWork Members

        public virtual IDbContextTransaction CurrentTransaction 
        { 
            get
            {
                return this.dbContext.Database.CurrentTransaction;
            }
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            return this.dbContext.Database.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            this.dbContext.Database.CommitTransaction();
        }

        public virtual void RollbackTransaction()
        {
            this.dbContext.Database.RollbackTransaction();
        }

        public virtual int SaveChanges()
        {
            try
            {
                int retVal = this.dbContext.SaveChanges();
                return retVal;
            }
            catch (Exception ex)
            {
                //TODO:Logging
                throw;
            }
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var retVal = this.dbContext.SaveChangesAsync(cancellationToken);
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
                    dbContext.Dispose();
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
