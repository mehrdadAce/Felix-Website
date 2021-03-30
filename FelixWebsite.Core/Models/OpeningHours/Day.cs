using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.OpeningHours
{
    public class Day
    {
        [JsonProperty(PropertyName = "openings")]
        public IEnumerable<Opening> Openings { get; set; }
        [JsonProperty(PropertyName = "dayName")]
        public string DayName { get; set; }
    }
}