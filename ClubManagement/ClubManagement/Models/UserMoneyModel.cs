using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class UserMoneyModel : FirebaseModel
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("moneyId")]
        public int MoneyId { get; set; }
    }
}