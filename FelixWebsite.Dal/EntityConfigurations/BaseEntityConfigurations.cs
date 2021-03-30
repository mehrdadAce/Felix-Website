using System.Data.Entity.ModelConfiguration;
using FelixWebsite.Dal.Entities.Base;

namespace FelixWebsite.Dal.EntityConfigurations
{
    public class BaseEntityConfigurations<TEntity>: EntityTypeConfiguration<TEntity> where TEntity: BaseEntity
    {
        public BaseEntityConfigurations()
        {
            HasKey(entity => entity.Id);

            Property(entity => entity.Id)
                .IsRequired();
        }
    }
}
