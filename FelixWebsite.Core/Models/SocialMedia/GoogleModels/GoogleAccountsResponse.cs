using Newtonsoft.Json;
using System.Collections.Generic;

namespace FelixWebsite.Core.Models.SocialMedia.GoogleModels
{
    public class State
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }

    public class Account
    {
        [JsonProperty(PropertyName = "state")]
        public State State { get; set; }

        [JsonProperty(PropertyName = "profilePhotoUrl")]
        public string ProfilePhotoUrl { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        public string AccountId => Name == null ? string.Empty : Name.Substring(Name.IndexOf('/') + 1);
    }

    public class GoogleAccountResponse
    {

        [JsonProperty(PropertyName = "accounts")]
        public List<Account> Accounts { get; set; }
    }
}
