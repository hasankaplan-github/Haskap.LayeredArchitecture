using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.BussinessLogicLayer.Services
{
    public class VisitIdProviderService
    {
        public VisitIdProviderService()
        {
            VisitId = Guid.NewGuid();
        }


        public Guid VisitId { get; }
    }
}
