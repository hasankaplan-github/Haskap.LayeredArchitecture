using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Entities
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
