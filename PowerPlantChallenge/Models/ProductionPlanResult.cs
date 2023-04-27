using System.Text.Json.Serialization;

namespace PowerPlantChallenge.API.Models
{
    public class ProductionPlanResult
    {
        [JsonPropertyName("name")]
        public string PowerPlantName { get; set; }
        [JsonPropertyName("p")]
        public decimal Power { get; set; }
        [JsonIgnore]
        public string PowerPlantType { get; set; }
    }
}
