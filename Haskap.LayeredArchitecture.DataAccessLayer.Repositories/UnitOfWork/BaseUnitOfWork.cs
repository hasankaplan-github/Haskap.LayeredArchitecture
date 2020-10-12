using Haskap.LayeredArchitecture.Core.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.DataAccessLayer.Repositories.UnitOfWork
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
