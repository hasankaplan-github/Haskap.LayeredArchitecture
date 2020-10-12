using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Entities
{
    public interface IHasClusteredIndex
    {
        long ClusteredIndex { get; set; }
    }
}
