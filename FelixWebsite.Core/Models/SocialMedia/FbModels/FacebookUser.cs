using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "picture")]
        public FaceBookPictureData Picture { get; set; }
    }
}