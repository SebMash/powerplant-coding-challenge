using PowerPlantChallenge.API.Services.Interfaces;
using PowerPlantChallenge.API.Constants;
using PowerPlantChallenge.API.Models;
using System;

namespace PowerPlantChallenge.API.Services
{
    public class PowerPlantCostCalculatorService : IPowerPlantCostCalculatorService
    {
        public decimal ComputeCostFor1Mhw(FuelData fuelData, PowerPlant powerplant)
        {
            switch (powerplant.Type)
            {
                case PowerPlantType.WindTurbine:
                    return 0;

                case PowerPlantType.GasFired:
                    return powerplant.Efficiency <= 0 ? 0 : fuelData.Kerosine / powerplant.Efficiency;

                case PowerPlantType.TurboJet:
                    return powerplant.Efficiency <= 0 ? 0 : fuelData.Gas / powerplant.Efficiency;

                default:
                    throw new ArgumentOutOfRangeException(powerplant.Type, $"The powerplant type {powerplant.Type} is not recognized");
            }
        }
    }
}
