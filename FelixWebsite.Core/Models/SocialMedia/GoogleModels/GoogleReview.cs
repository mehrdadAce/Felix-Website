using System;
using Newtonsoft.Json;


namespace FelixWebsite.Core.Models.SocialMedia.GoogleModels
{
    public class GoogleReview: BaseReview
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "reviewId")]
        public string ReviewId { get; set; }

        [JsonProperty(PropertyName = "reviewer")]
        public GoogleReviewer Reviewer { get; set; }

        [JsonProperty(PropertyName = "starRating")]
        public string GoogleStarRating { get; set; }

        public GoogleStarRating Rating
        {
            get
            {
                switch (GoogleStarRating?.ToUpper())
                {
                    case "FIVE":
                        return GoogleModels.GoogleStarRating.Five;
                    case "FOUR":
                        return GoogleModels.GoogleStarRating.Four;
                    case "ONE":
                        return GoogleModels.GoogleStarRating.One;
                    case "THREE":
                        return GoogleModels.GoogleStarRating.Three;
                    case "TWO":
                        return GoogleModels.GoogleStarRating.Two;
                    default:
                        return GoogleModels.GoogleStarRating.StarRatingUnspecified;
                }
            }
        }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "createTime")]
        public string CreatedTime { get; set; }
        public DateTime CreatedDateTime => DateTime.Parse(CreatedTime);

        [JsonProperty(PropertyName = "updateTime")]
        public string UpdateTime { get; set; }
        public DateTime UpdatedDateTime => DateTime.Parse(UpdateTime);

        [JsonProperty(PropertyName = "reviewReply")]
        public GoogleReviewReply ReviewReply { get; set; }
    }
}
