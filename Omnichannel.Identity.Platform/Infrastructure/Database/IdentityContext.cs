using Core.Framework.Repository;
using Microsoft.EntityFrameworkCore;
using Omnichannel.Identity.Platform.Infrastructure.Database.Configurations;

namespace Omnichannel.Identity.Platform.Infrastructure.Database
{
    public class IdentityContext : BaseContext
    {
        public IdentityContext(IDataSource source)
            : base(source)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
