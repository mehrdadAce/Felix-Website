using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FaceBookPictureData
    {
        [JsonProperty(PropertyName = "data")]
        public FaceBookPicture FacebookPicture { get; set; }

    }
}