using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Entities
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
