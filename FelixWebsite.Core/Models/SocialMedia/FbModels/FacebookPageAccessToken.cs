using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookPageAccessToken
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}