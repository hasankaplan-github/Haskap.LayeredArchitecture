using Haskap.LayeredArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Haskap.LayeredArchitecture.DataAccessLayer.DbContexts.Configurations
{
    public class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity> 
        where TEntity : BaseEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            
            if (typeof(IHasClusteredIndex).IsAssignableFrom(typeof(TEntity)))
            {
                builder.HasKey(x => x.Id).IsClustered(false);
                builder.HasIndex(x => (x as IHasClusteredIndex).ClusteredIndex).IsClustered();
            }

            //builder.Property(x => x.ClusteredIndex).UseIdentityAlwaysColumn();
            //builder.ForNpgsqlHasIndex(x => x.ClusteredIndex).IsUnique();

            //builder.HasMany(x => x.RecordLogs)
            //    .WithOne()
            //    .HasForeignKey(x => x.RecordId)
            //    .OnDelete(DeleteBehavior.Cascade); // bu relation drop edilen kaydın logları silinmemesi için commentlendi. 
        }
    }
}
