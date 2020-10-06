using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Entites
{
    public interface IClusteredIndexable
    {
        long ClusteredIndex { get; set; }
    }
}
