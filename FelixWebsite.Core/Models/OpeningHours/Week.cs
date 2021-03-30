using System.Collections.Generic;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.OpeningHours
{
    public class Week
    {
        [JsonProperty(PropertyName = "days")]
        public IEnumerable<Day> Days { get; set; }
    }
}
