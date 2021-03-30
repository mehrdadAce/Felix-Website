using FelixWebsite.Bdo.Models.Base;
using FelixWebsite.Core.enums;

namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class PhotoInfo:BaseModel
    {
        public int MediaId { get; set; }
        public int UserId { get; set; }
        public PhotoIdentifier PhotoIdentifier { get; set; }
        public PhotoInfo(int mediaId, int userId, PhotoIdentifier photoIdentifier)
        {
            MediaId = mediaId;
            UserId = userId;
            PhotoIdentifier = photoIdentifier;
        }
    }
}