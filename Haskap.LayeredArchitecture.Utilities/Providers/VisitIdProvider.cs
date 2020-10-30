using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Utilities.Providers
{
    public class VisitIdProvider
    {
        public VisitIdProvider()
        {
            VisitId = Guid.NewGuid();
        }


        public Guid VisitId { get; }
    }
}
