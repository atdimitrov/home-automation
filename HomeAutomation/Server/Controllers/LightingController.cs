using HomeAutomation.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeAutomation.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LightingController : ControllerBase
    {
        private readonly ILightingService lightingService;

        public LightingController(ILightingService lightingService)
        {
            this.lightingService = lightingService;
        }

        [HttpGet]
        public IActionResult GetState() => this.Ok(this.lightingService.GetState());

        [HttpPost]
        public void TurnOn() => this.lightingService.TurnOn();

        [HttpPost]
        public void TurnOff() => this.lightingService.TurnOff();
    }
}
