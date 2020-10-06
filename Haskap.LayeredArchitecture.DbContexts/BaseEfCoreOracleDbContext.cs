﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using Haskap.LayeredArchitecture.Utilities.ExtensionMethods;

namespace Haskap.LayeredArchitecture.DbContexts
{
    public class BaseEfCoreOracleDbContext : DbContext
    {
        public BaseEfCoreOracleDbContext(DbContextOptions<BaseEfCoreOracleDbContext> options) : base(options)
        {
        }

        protected BaseEfCoreOracleDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName().ToSnakeCase(CaseOption.UpperCase));

                foreach (var property in entityType.GetProperties())
                    property.SetColumnName(property.GetColumnName().ToSnakeCase(CaseOption.UpperCase));

                foreach (var key in entityType.GetKeys())
                    key.SetName(key.GetName().ToSnakeCase(CaseOption.UpperCase));

                foreach (var foreignKey in entityType.GetForeignKeys())
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase(CaseOption.UpperCase));

                foreach (var index in entityType.GetIndexes())
                    index.SetName(index.GetName().ToSnakeCase(CaseOption.UpperCase));
            }

            //builder.ApplyConfiguration(new MessageConfigurations());
            //builder.ApplyConfiguration(new LogConfigurations());
            //builder.ApplyConfiguration(new LogDetailConfigurations());
            //builder.ApplyConfiguration(new UserConfigurations());
        }
    }
}
