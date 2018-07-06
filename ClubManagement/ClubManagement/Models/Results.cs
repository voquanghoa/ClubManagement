using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class Results
    {
        [JsonProperty("results")]
        public List<Address> Addresses { get; set; }
    }
}