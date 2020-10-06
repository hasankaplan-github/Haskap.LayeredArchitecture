using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Entites
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
