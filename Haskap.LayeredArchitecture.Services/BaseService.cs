using Haskap.LayeredArchitecture.Core.Repositories.UnitOfWork;
using Haskap.LayeredArchitecture.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Services
{
    public class BaseService<TUnitOfWork> : IBaseService<TUnitOfWork>
        where TUnitOfWork : IBaseUnitOfWork
    {
        public BaseService(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public TUnitOfWork UnitOfWork { get; set; }
    }
}
