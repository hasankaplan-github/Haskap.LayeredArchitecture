using Haskap.LayeredArchitecture.Core.Services;
using Haskap.LayeredArchitecture.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.BussinessLogicLayer.Services
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
