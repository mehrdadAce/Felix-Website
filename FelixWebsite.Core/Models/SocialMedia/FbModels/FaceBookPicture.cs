using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FaceBookPicture
    {
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }
        [JsonProperty(PropertyName = "is_silhouette")]
        public bool IsSilhouette { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}