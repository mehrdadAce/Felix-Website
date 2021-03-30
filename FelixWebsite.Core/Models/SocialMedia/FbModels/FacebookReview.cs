using System;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookReview: BaseReview
    {
        [JsonProperty(PropertyName = "created_time")]
        public string CreatedTime { get; set; }

        public DateTime CreatedDateTime => DateTime.Parse(CreatedTime);

        [JsonProperty(PropertyName = "recommendation_type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "review_text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "reviewer")]
        public FacebookUser FbUser { get; set; }


        [JsonProperty(PropertyName = "has_rating")]
        public bool HasRating { get; set; }


        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }


        public string StarRating
        {
            get
            {
                if (!HasRating)
                {
                    var random = new Random();
                    Rating = random.Next(3, 5);
                }

                return Rating.ToString();
            }
        }
    }
}