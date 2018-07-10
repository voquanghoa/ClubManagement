using System;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class DistanceMatrixDistanceAPIModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }
}