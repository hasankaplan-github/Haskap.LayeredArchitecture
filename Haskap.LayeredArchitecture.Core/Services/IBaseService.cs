using Haskap.LayeredArchitecture.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Services
{
    public interface IBaseService<TUnitOfWork>
        where TUnitOfWork : IBaseUnitOfWork
    {
        TUnitOfWork UnitOfWork { get; set; }
    }
}
