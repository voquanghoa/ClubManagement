using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class UserEventModel : FirebaseModel
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("eventid")]
        public string EventId { get; set; }
    }
}