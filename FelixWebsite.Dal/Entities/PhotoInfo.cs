using FelixWebsite.Dal.Entities.Base;
using FelixWebsite.Core.enums;

namespace FelixWebsite.Dal.Entities
{
    public class PhotoInfo:BaseEntity
    {
        public int MediaId { get; set; }
        public int UserId { get; set; }
        public PhotoIdentifier PhotoIdentifier { get; set; }
    }
}
