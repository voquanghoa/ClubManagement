using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ClubManagement.Models
{
    public class NotificationModel : FirebaseModel
    {
        [JsonProperty("typeId")]
        public string TypeId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("toUserIds")]
        public List<string> ToUserIds { get; set; } = new List<string>();

        [JsonProperty("userIdsSeen")]
        public List<string> UserIdsSeen { get; set; } = new List<string>();

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }
    }
}