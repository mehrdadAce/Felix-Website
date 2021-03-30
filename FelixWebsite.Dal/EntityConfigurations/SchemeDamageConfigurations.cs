using FelixWebsite.Dal.Entities;

namespace FelixWebsite.Dal.EntityConfigurations
{
    public class SchemeDamageConfigurations: BaseEntityConfigurations<SchemeDamage>
    {
        public SchemeDamageConfigurations()
        {
            Property(ent => ent.UserId).
                IsRequired();
            Property(ent => ent.SchemeEntry)
                .IsRequired();
            Property(ent => ent.Damage)
                .IsOptional();
            Property(ent => ent.HeavyDamage)
                .IsOptional();
            Property(ent => ent.HailDamage)
                .IsOptional();
            Property(ent => ent.Marks)
                .IsOptional();
            Property(ent => ent.Dent)
                .IsOptional();
            Property(ent => ent.Remarks)
                .IsOptional();
        }
    }
}
