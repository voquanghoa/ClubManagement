using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class OutcomeAmountItem : FirebaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }
}