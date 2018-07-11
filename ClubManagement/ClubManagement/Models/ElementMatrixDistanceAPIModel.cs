using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class ElementMatrixDistanceAPIModel
    {
        [JsonProperty("distance")]
        public DistanceMatrixDistanceAPIModel Distance { get; set; }

        [JsonProperty("duration")]
        public DistanceMatrixDistanceAPIModel Duration { get; set; }
    }
}