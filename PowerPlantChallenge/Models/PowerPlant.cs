using System.Text.Json.Serialization;

namespace PowerPlantChallenge.API.Models
{
    public class PowerPlant
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Efficiency { get; set; }
        public decimal Pmin { get; set; }
        public decimal Pmax { get; set; }

        [JsonIgnore]
        public decimal CostFor1Mhw { get; set; }
    }
}
