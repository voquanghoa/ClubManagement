using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class FirebaseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}