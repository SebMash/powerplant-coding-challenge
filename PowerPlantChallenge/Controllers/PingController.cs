using Microsoft.AspNetCore.Mvc;

namespace PowerPlantChallenge.API.Controllers
{
    [Route("[controller]")]
    public class PingController : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pong!");
        }
    }
}
