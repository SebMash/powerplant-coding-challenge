using System.Collections.Generic;

namespace PowerPlantChallenge.API.Models
{
    public class PayLoad
    {
        public decimal Load { get; set; }
        public FuelData Fuels { get; set; }
        public IEnumerable<PowerPlant> PowerPlants { get; set; }
    }
}
