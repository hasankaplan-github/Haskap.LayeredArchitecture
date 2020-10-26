using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.UnitOfWork
{
    public interface IBaseUnitOfWork : IDisposable
    {
        int SaveChanges();

        IDbContextTransaction CurrentTransaction { get; }
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
