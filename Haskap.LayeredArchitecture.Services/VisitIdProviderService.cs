using System;

namespace Haskap.LayeredArchitecture.Services
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
