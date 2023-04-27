using PowerPlantChallenge.API.Models;

namespace PowerPlantChallenge.API.Services.Interfaces
{
    public interface IPowerPlantCostCalculatorService
    {
        decimal ComputeCostFor1Mhw(FuelData fuelData, PowerPlant powerplant);
    }
}
