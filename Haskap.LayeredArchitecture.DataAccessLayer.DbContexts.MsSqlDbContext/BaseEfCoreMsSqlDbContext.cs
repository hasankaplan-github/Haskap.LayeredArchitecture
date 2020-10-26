using Microsoft.EntityFrameworkCore;
using System;

namespace Haskap.LayeredArchitecture.DataAccessLayer.DbContexts.MsSqlDbContext
{
    public class BaseEfCoreMsSqlDbContext : DbContext
    {
        public BaseEfCoreMsSqlDbContext(DbContextOptions<BaseEfCoreMsSqlDbContext> options) : base(options)
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
