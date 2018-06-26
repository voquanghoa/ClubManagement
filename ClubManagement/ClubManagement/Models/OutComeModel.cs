using System;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class OutComeModel : FirebaseModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}