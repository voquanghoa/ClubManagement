using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class RowMatrixDistanceAPIModel
    {
        [JsonProperty("elements")]
        public List<ElementMatrixDistanceAPIModel> Elements { get; set; }
    }
}