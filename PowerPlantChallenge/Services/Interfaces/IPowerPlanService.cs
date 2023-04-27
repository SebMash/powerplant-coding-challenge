using PowerPlantChallenge.API.Models;
using System.Collections.Generic;

namespace PowerPlantChallenge.API.Services.Interfaces
{
    public interface IPowerPlanService
    {
        List<ProductionPlanResult> ComputePowerPlan(PayLoad payload);
    }
}
