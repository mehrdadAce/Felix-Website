using System.Data.Entity;
using EntityFramework.DynamicFilters;
using FelixWebsite.Dal.Entities;
using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Dal.EntityConfigurations;

namespace FelixWebsite.Dal.FelixEntities
{
    public class FelixEntities:DbContext
    {
        public DbSet<PhotoInfo> PhotoIds { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        
        public DbSet<SchemeDamage> SchemeDamages { get; set; }

        public FelixEntities():base("name=umbracoDbDSN")
        {
            var ensureDllIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FelixEntities, Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PhotoIdsConfigurations());
            modelBuilder.Configurations.Add(new UserInformationConfigurations());
            modelBuilder.Configurations.Add(new SchemeDamageConfigurations());
            modelBuilder.Filter("IsDeleted", (BaseEntity e) => e.AuditDeleted, false);
        }
    }
}
