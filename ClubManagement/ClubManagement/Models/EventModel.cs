using System;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class EventModel : FirebaseModel
    {
        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("time")]
        public DateTime Time { get; set; } = new DateTime();

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; } = "";

        [JsonProperty("createdTime")]
        public string CreatedTime { get; set; } = "";

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}