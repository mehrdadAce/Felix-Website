using System;
using System.Collections.Generic;
using FelixWebsite.Core.Models.SocialMedia.Interfaces;
using Newtonsoft.Json;

namespace FelixWebsite.Core.Models.SocialMedia.FbModels
{
    public class FacebookPost: BaseArticle, ISelectable
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public object Actions { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public object Attachments { get; set; }

        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }

        [JsonProperty(PropertyName = "child_attachments")]
        public object ChildAttachments { get; set; }

        [JsonProperty(PropertyName = "created_time")]
        public string CreatedTime { get; set; }
        public DateTime CreatedDateTime => DateTime.Parse(CreatedTime);

        [JsonProperty(PropertyName = "from")]
        public Creator User { get; set; }

        [JsonProperty(PropertyName = "full_picture")]
        public string PhotoUrl { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "is_hidden")]
        public string IsHidden { get; set; }

        [JsonProperty(PropertyName = "is_published")]
        public string IsPublished { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string StatusMessage { get; set; }

        [JsonProperty(PropertyName = "message_tags")]
        public IEnumerable<object>MessageTags { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string LinkName { get; set; }

        [JsonProperty(PropertyName = "object_id")]
        public string ObjectId { get; set; }

        [JsonProperty(PropertyName = "parent_id")]
        public string ParentId { get; set; }

        [JsonProperty(PropertyName = "permalink_url")]
        public string PermalinkUrl { get; set; }

        [JsonProperty(PropertyName = "picture")]
        public string Picture { get; set; }

        [JsonProperty(PropertyName = "place")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "privacy")]
        public object Privacy { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public IEnumerable<object> Properties { get; set; }

        [JsonProperty(PropertyName = "shares")]
        public object Shares { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string SourceUrl { get; set; }

        [JsonProperty(PropertyName = "targeting")]
        public string Targeting { get; set; }

        [JsonProperty(PropertyName = "to")]
        public IEnumerable<object> MentionedProfiles { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public bool Selected { get; set; }
    }
}
//https://developers.facebook.com/docs/graph-api/reference/v3.2/post