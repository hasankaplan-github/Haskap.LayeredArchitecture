using Haskap.LayeredArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Haskap.LayeredArchitecture.DataAccess.EntityConfiguration
{
    public class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                builder.HasQueryFilter(x => (x as ISoftDeletable).IsDeleted == false);
            }
        }
    }
}
