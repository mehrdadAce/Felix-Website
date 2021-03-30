using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.GoogleModels
{
    public class GoogleReviewReply
    {
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "updateTime")]
        public string UpdateTime { get; set; }
    }
}
