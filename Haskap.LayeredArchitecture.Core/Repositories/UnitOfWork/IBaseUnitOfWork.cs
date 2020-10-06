using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Repositories.UnitOfWork
{
    public interface IBaseUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}
