using Haskap.LayeredArchitecture.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Haskap.LayeredArchitecture.Core.Specifications
{
    public abstract class Specification<TEntity, TId>
        where TEntity : BaseEntity<TId>
    {
        private Func<TEntity, bool> predicateCache = null;

        public virtual bool IsSatisfiedBy(TEntity entity)
        {
            predicateCache ??= ToExpression().Compile();
            return predicateCache(entity);
        }

        public abstract Expression<Func<TEntity, bool>> ToExpression();

        public static implicit operator Expression<Func<TEntity, bool>>(Specification<TEntity, TId> specification)
        {
            return specification.ToExpression();
        }
    }
}
