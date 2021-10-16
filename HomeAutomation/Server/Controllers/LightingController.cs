using HomeAutomation.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeAutomation.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LightingController : ControllerBase
    {
        private readonly ILightingService lightingService;
        private readonly ILightingStateService lightingStateService;

        public LightingController(ILightingService lightingService, ILightingStateService lightingStateService)
        {
            this.lightingService = lightingService;
            this.lightingStateService = lightingStateService;
        }

        [HttpGet]
        public IActionResult GetState() => this.Ok(this.lightingService.GetState());

        [HttpGet]
        public IActionResult GetUpcomingStateChanges() =>
            this.Ok(this.lightingStateService.GetUpcomingLightingStateChanges());
    }
}
