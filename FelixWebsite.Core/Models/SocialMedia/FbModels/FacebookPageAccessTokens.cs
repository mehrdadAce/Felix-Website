using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookPageAccessTokens
    {
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<FacebookPageAccessToken> PageTokensList { get; set; }
    }
}
