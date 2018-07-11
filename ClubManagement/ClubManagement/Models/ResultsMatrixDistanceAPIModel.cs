using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClubManagement.Models
{
    public class ResultsMatrixDistanceAPIModel
    {
        [JsonProperty("rows")]
        public List<RowMatrixDistanceAPIModel> Rows { get; set; }
    }
}