using System;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class MoneyModel : FirebaseModel
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }
    }
}