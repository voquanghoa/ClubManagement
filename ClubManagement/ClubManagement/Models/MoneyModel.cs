using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class MoneyModel : FirebaseModel
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}