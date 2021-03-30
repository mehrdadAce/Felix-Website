using Newtonsoft.Json;
using System.Collections.Generic;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookResponse
    {
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<FacebookPost> Data { get; set; }
    }
}
