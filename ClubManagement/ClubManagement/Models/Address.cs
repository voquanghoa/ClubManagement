using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class Address
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
    }
}