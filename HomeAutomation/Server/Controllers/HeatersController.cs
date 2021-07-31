using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HomeAutomation.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HeatersController : ControllerBase
    {
        private readonly IHeaterService heaterService;

        public HeatersController(IHeaterService heaterService)
        {
            this.heaterService = heaterService;
        }

        [HttpGet]
        public IActionResult Get() => this.Ok(this.heaterService.GetHeaters());

        [HttpPost]
        public void ToggleHeater([FromBody] Heater heater) => this.heaterService.ToggleHeater(heater);
    }
}
