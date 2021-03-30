using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.GoogleModels
{
    public class GoogleReviewer
    {
        [JsonProperty(PropertyName = "profilePhotoUrl")]
        public string ProfilePhotoUrl { get; set; }
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "isAnonymous")]
        public bool IsAnonymous { get; set; }

    }
}
