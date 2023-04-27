using Microsoft.Extensions.Logging;
using PowerPlantChallenge.API.Services.Interfaces;
using PowerPlantChallenge.API.Constants;
using PowerPlantChallenge.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerPlantChallenge.API.Services
{
    public class PowerPlanService : IPowerPlanService
    {
        private readonly IPowerPlantCostCalculatorService _costService;
        private readonly ILogger<PowerPlanService> _logger;
        public PowerPlanService(IPowerPlantCostCalculatorService costService, ILogger<PowerPlanService> logger)
        {
            _costService = costService;
            _logger = logger;
        }

        public List<ProductionPlanResult> ComputePowerPlan(PayLoad payload)
        {
            var result = new List<ProductionPlanResult>();

            if (payload == null || !payload.PowerPlants?.Any() == true || payload.Fuels == null || payload.Load <= 0) return result;

            var powerPlants = payload.PowerPlants
            .ToList();

            _logger.LogInformation($"{nameof(PowerPlantCostCalculatorService.ComputeCostFor1Mhw)} - Start cost algorithm");
            try
            {
                //compute the cost for each powerplant
                powerPlants.ForEach(x =>
                {
                    x.CostFor1Mhw = _costService.ComputeCostFor1Mhw(payload.Fuels, x);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(PowerPlanService.ComputePowerPlan)} - An error occured while computing the cost of powerplants.");
                return result;
            }
            _logger.LogInformation($"{nameof(PowerPlantCostCalculatorService.ComputeCostFor1Mhw)} - End cost algorithm");

            _logger.LogInformation($"{nameof(PowerPlanService.ComputePowerPlan)} - Start powerplan");
            //we tackle the power generation by order of the most profitable powerplants
            try
            {
                foreach (var powerplant in powerPlants.OrderBy(x => x.CostFor1Mhw))
                {
                    var productionPlantResult = new ProductionPlanResult()
                    {
                        PowerPlantName = powerplant.Name,
                        PowerPlantType = powerplant.Type
                    };

                    var remainingPowerloadNeeded = payload.Load - result.Sum(p => p.Power);
                    switch (powerplant.Type)
                    {
                        case PowerPlantType.WindTurbine:
                            var maxPowerThreshold = powerplant.Efficiency <= 0 ? 0 : (payload.Fuels.WindPercentage / 100) * powerplant.Pmax;
                            var maxProductionNeeded = Math.Min(maxPowerThreshold, remainingPowerloadNeeded);
                            productionPlantResult.Power = maxProductionNeeded <= 0 ? 0 : maxProductionNeeded > powerplant.Pmin ? maxProductionNeeded : powerplant.Pmin;
                            break;
                        case PowerPlantType.GasFired:
                        case PowerPlantType.TurboJet:
                            maxPowerThreshold = powerplant.Efficiency <= 0 ? 0 : powerplant.Pmax;
                            maxProductionNeeded = Math.Min(maxPowerThreshold, remainingPowerloadNeeded);
                            productionPlantResult.Power = maxProductionNeeded <= 0 ? 0 : maxProductionNeeded > powerplant.Pmin ? maxProductionNeeded : powerplant.Pmin;
                            break;

                        default:
                            break;
                    }

                    result.Add(productionPlantResult);
                }

                result = RemoveUnnecessaryPowerPlants(result, payload.Load);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(PowerPlanService.ComputePowerPlan)} - An error occured while computing the powerplan of powerplants.");
                throw; 
            }
            _logger.LogInformation($"{nameof(PowerPlanService.ComputePowerPlan)} - End powerplan");

            return result
                .ToList();
        }

        //will remove smallests powerplants if the other ones can coverup the powerload needed with their pMin or pMax.
        private List<ProductionPlanResult> RemoveUnnecessaryPowerPlants(List<ProductionPlanResult> productionPlan, decimal powerLoad)
        {
            //we ignore windturbines as they dont cost anything and powerplants without power generated
            var smallestProductionPlant = productionPlan.OrderBy(a => a.Power).FirstOrDefault(x => x.PowerPlantType != PowerPlantType.WindTurbine && x.Power > 0);
            if (smallestProductionPlant == null) return productionPlan;

            var powerLoadWithouhtSmallestPowerPlant = productionPlan.Sum(p => p.Power) - smallestProductionPlant.Power;

            while (productionPlan.Count > 1 && powerLoadWithouhtSmallestPowerPlant >= powerLoad)
            {
                //we set the power to 0
                var index = productionPlan.IndexOf(smallestProductionPlant);
                productionPlan[index].Power = 0;
                smallestProductionPlant = productionPlan.OrderBy(a => a.Power).FirstOrDefault(x => x.PowerPlantType != PowerPlantType.WindTurbine && x.Power > 0);
                powerLoadWithouhtSmallestPowerPlant = productionPlan.Sum(p => p.Power) - smallestProductionPlant?.Power ?? 0;
            }

            return productionPlan;
        }
    }
}
