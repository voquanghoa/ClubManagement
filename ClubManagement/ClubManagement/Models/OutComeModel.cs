using System;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class OutcomeModel : FirebaseModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }
    }
}