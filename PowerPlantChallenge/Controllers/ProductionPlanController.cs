using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PowerPlantChallenge.API.Services.Interfaces;
using PowerPlantChallenge.API.Models;

namespace PowerPlantChallenge.API.Controllers
{
    [Route("[controller]")]
    public class ProductionPlanController : BaseController
    {
        private readonly IPowerPlanService _powerplanService;
        private readonly ILogger<ProductionPlanController> _logger;
        public ProductionPlanController(IPowerPlanService powerplanService, ILogger<ProductionPlanController> logger)
        {
            _powerplanService = powerplanService;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult Post([FromBody] PayLoad payload)
        {
            _logger.LogInformation($"Received {nameof(ProductionPlanController.Post)} request.");

            var result = _powerplanService.ComputePowerPlan(payload);

            return Ok(result);
        }
    }
}
