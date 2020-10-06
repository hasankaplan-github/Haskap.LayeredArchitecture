using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using Haskap.LayeredArchitecture.Utilities.ExtensionMethods;

namespace Haskap.LayeredArchitecture.DbContexts
{
    public class BaseEfCoreNpgsqlDbContext : DbContext
    {
        public BaseEfCoreNpgsqlDbContext(DbContextOptions<BaseEfCoreNpgsqlDbContext> options) : base(options)
        {
        }

        protected BaseEfCoreNpgsqlDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName().ToSnakeCase(CaseOption.LowerCase));

                foreach (var property in entityType.GetProperties())
                    property.SetColumnName(property.GetColumnName().ToSnakeCase(CaseOption.LowerCase));

                foreach (var key in entityType.GetKeys())
                    key.SetName(key.GetName().ToSnakeCase(CaseOption.LowerCase));

                foreach (var foreignKey in entityType.GetForeignKeys())
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase(CaseOption.LowerCase));

                foreach (var index in entityType.GetIndexes())
                    index.SetName(index.GetName().ToSnakeCase(CaseOption.LowerCase));
            }

            //builder.ApplyConfiguration(new MessageConfigurations());
            //builder.ApplyConfiguration(new LogConfigurations());
            //builder.ApplyConfiguration(new LogDetailConfigurations());
            //builder.ApplyConfiguration(new UserConfigurations());
        }
    }
}
