using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.LayeredArchitecture.DataAccessLayer.DbContexts
{
    public class BaseEfCoreMsSqlDbContext : DbContext
    {
        public BaseEfCoreMsSqlDbContext(DbContextOptions<BaseEfCoreNpgsqlDbContext> options) : base(options)
        {
        }

        protected BaseEfCoreMsSqlDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.ApplyConfiguration(new MessageConfigurations());
            //builder.ApplyConfiguration(new LogConfigurations());
            //builder.ApplyConfiguration(new LogDetailConfigurations());
            //builder.ApplyConfiguration(new UserConfigurations());
        }
    }
}
