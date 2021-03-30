using System;
using System.Globalization;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.OpeningHours
{
    public class Opening
    {
        [JsonProperty(PropertyName =  "start")]
        public string Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }
        public TimeSpan StartHour => TimeSpan.Parse(Start);
        public TimeSpan EndHour => TimeSpan.Parse(End);
        [JsonProperty(PropertyName = "isOpen")]
        public bool IsOpen { get; set; }
    }
}
