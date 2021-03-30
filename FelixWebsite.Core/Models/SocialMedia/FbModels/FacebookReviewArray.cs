using System.Collections.Generic;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookReviewArray
    {
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<FacebookReview> Data { get; set; }
    }
}