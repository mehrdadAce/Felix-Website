using System.Collections.Generic;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.GoogleModels
{
    public class GoogleReviewResponse
    {
        [JsonProperty(PropertyName = "reviews")]
        public List<GoogleReview> Reviews { get; set; }

        [JsonProperty(PropertyName = "averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty(PropertyName = "totalReviewCount")]
        public int TotalReviewCount { get; set; }
    }
}
