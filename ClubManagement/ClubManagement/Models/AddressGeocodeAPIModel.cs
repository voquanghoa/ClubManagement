using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class AddressGeocodeAPIModel
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
    }
}