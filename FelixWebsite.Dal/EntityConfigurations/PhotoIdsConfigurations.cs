using FelixWebsite.Dal.Entities;

namespace FelixWebsite.Dal.EntityConfigurations
{
    public class PhotoIdsConfigurations:BaseEntityConfigurations<PhotoInfo>
    {
        public PhotoIdsConfigurations()
        {
            Property(ent => ent.UserId).
                IsRequired();
            Property(ent => ent.MediaId)
                .IsRequired();
            Property(ent => ent.PhotoIdentifier)
                .IsRequired();
        }
    }
}
