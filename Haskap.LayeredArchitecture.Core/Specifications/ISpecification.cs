using Haskap.LayeredArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Specifications
{
    public interface ISpecification<TEntity, TId>
        where TEntity : BaseEntity<TId>
    {
        bool IsSatisfiedBy(TEntity entity);
    }
}
