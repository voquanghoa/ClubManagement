using Newtonsoft.Json;
using System;

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

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }
    }
}