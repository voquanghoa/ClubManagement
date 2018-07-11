using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class ResultsGeocodeAPIModel
    {
        [JsonProperty("results")]
        public List<AddressGeocodeAPIModel> Addresses { get; set; }
    }
}