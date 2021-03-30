namespace FelixWebsite.Bdo.Models.Acquisition
{
    public class MediaValuesModel
    {
        public MediaValuesModel(int mediaId, string mediaName, string mediaUrl)
        {
            MediaId = mediaId;
            MediaName = mediaName;
            MediaUrl = mediaUrl;
        }

        public int MediaId { get; set; }

        public string MediaUrl { get; set; }

        public string MediaName { get; set; }

        public DamageEntry GetDamageEntry(bool isSchemePicture = false)
        {
            return new DamageEntry(MediaUrl, MediaName, MediaId, isSchemePicture);
        }
    }
}
