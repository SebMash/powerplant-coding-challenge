using Microsoft.Extensions.Logging;
using Moq;
using PowerPlantChallenge.API.Services;
using PowerPlantChallenge.API.Services.Interfaces;
using PowerPlantChallenge.API.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace PowerPlantChallenge.Tests
{
    public class PowerPlanServiceTests
    {
        private readonly Mock<ILogger<PowerPlanService>> _mockLogger;

        public PowerPlanServiceTests()
        {
            _mockLogger = new Mock<ILogger<PowerPlanService>>();
        }

        [Theory]
        [InlineData("./files/payload1.json")]
        [InlineData("./files/payload2.json")]
        [InlineData("./files/payload3.json")]
        public void ComputePowerPlan_WithCorrectData_ShouldReturnExactSameNumberOfPowerPlants(string file)
        {
            // Arrange
            var fileJson = File.ReadAllText(file);
            var payload = System.Text.Json.JsonSerializer.Deserialize<PayLoad>(fileJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            IPowerPlantCostCalculatorService costService = new PowerPlantCostCalculatorService();
            IPowerPlanService service = new PowerPlanService(costService, _mockLogger.Object);
            //act
            var result = service.ComputePowerPlan(payload);

            //assert
            Assert.Equal(payload.PowerPlants.Count(), result.Count());
        }

        [Theory]
        [InlineData("./files/payload2.json")]
        public void ComputePowerPlan_WithNoWind_ShouldReturnZeroPowerForWindTurbines(string file)
        {
            // Arrange
            var fileJson = File.ReadAllText(file);
            var payload = System.Text.Json.JsonSerializer.Deserialize<PayLoad>(fileJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            IPowerPlantCostCalculatorService costService = new PowerPlantCostCalculatorService();
            IPowerPlanService service = new PowerPlanService(costService, _mockLogger.Object);
            //act
            var result = service.ComputePowerPlan(payload);

            var firstWT = result.First(x => x.PowerPlantName == "windpark1");
            var secondWT = result.First(x => x.PowerPlantName == "windpark2");
            //assert
            Assert.Equal(0, firstWT.Power);
            Assert.Equal(0, secondWT.Power);
        }

        [Theory]
        [InlineData("./files/payload4.json")]
        public void ComputePowerPlan_PowerPlantWithNoEfficiency_ShouldReturnZeroPower(string file)
        {
            // Arrange
            var fileJson = File.ReadAllText(file);
            var payload = System.Text.Json.JsonSerializer.Deserialize<PayLoad>(fileJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            IPowerPlantCostCalculatorService costService = new PowerPlantCostCalculatorService();
            IPowerPlanService service = new PowerPlanService(costService, _mockLogger.Object);
            //act
            var result = service.ComputePowerPlan(payload);

            var firstWT = result.First(x => x.PowerPlantName == "gasfiredbig1");
            var secondWT = result.First(x => x.PowerPlantName == "gasfiredbig2");
            //assert
            
            Assert.Equal(0, firstWT.Power);
            Assert.Equal(0, secondWT.Power);
        }

        [Fact]
        public void ComputePowerPlan_WithoutPayLoad_ShouldReturnEmptyList()
        {
            // Arrange
            IPowerPlantCostCalculatorService costService = new PowerPlantCostCalculatorService();
            IPowerPlanService service = new PowerPlanService(costService, _mockLogger.Object);
            //act
            var result = service.ComputePowerPlan(null);

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public void ComputePowerPlan_WithZeroLoad_ShouldReturnEmptyList()
        {
            // Arrange
            var payload = new PayLoad()
            {
                Fuels = new FuelData(),
                Load = 0,
                PowerPlants = new List<PowerPlant>()
            };

            IPowerPlantCostCalculatorService costService = new PowerPlantCostCalculatorService();
            IPowerPlanService service = new PowerPlanService(costService, _mockLogger.Object);
            //act
            var result = service.ComputePowerPlan(payload);

            //assert
            Assert.Empty(result);
        }
    }
}
