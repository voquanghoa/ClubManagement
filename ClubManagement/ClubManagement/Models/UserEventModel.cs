using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class UserEventModel : FirebaseModel
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("eventid")]
        public int EventId { get; set; }
    }
}