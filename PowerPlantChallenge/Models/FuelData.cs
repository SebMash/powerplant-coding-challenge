using System.Text.Json.Serialization;

namespace PowerPlantChallenge.API.Models
{
    public class FuelData
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal Gas { get; set; }
        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal Kerosine { get; set; }
        [JsonPropertyName("co2(euro/ton)")]
        public decimal Co2 { get; set; }
        [JsonPropertyName("wind(%)")]
        public decimal WindPercentage { get; set; }
    }
}
