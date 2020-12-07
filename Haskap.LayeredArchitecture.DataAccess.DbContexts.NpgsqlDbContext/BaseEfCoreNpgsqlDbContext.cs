﻿using Haskap.LayeredArchitecture.Utilities.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using System;

namespace Haskap.LayeredArchitecture.DataAccess.DbContexts.NpgsqlDbContext
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
                    property.SetColumnName(property.Name.ToSnakeCase(CaseOption.LowerCase));

                //foreach (var key in entityType.GetKeys())
                //    key.SetName(key.GetName().ToSnakeCase(CaseOption.LowerCase));

                //foreach (var foreignKey in entityType.GetForeignKeys())
                //    foreignKey.PrincipalKey.SetName(foreignKey.PrincipalKey.GetName().ToSnakeCase(CaseOption.LowerCase));
                ////foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase(CaseOption.LowerCase));

                //foreach (var index in entityType.GetIndexes())
                //    index.SetName(index.GetName().ToSnakeCase(CaseOption.LowerCase));
            }

            //builder.ApplyConfiguration(new MessageConfigurations());
            //builder.ApplyConfiguration(new LogConfigurations());
            //builder.ApplyConfiguration(new LogDetailConfigurations());
            //builder.ApplyConfiguration(new UserConfigurations());
        }
    }
}
