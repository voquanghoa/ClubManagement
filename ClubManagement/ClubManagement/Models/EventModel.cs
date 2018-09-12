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

        [JsonProperty("timeStart")]
        public DateTime TimeStart { get; set; } = new DateTime();

        [JsonProperty("timeEnd")]
        public DateTime TimeEnd { get; set; } = new DateTime();

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; } = "";

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; } = new DateTime();

        [JsonProperty("place")]
        public string Place { get; set; } = "";

        [JsonProperty("address")]
        public string Address { get; set; } = "";

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}