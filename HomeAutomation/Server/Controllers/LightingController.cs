using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HomeAutomation.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LightingController : ControllerBase
    {
        private readonly ILightingService lightingService;
        private readonly ISolarEventsService solarEventsService;

        public LightingController(ILightingService lightingService, ISolarEventsService solarEventsService)
        {
            this.lightingService = lightingService;
            this.solarEventsService = solarEventsService;
        }

        [HttpGet]
        public IActionResult GetState() => this.Ok(this.lightingService.GetState());

        [HttpGet]
        public IActionResult GetUpcomingStateChanges()
        {
            UpcomingSolarEvents upcomingSolarEvents = this.solarEventsService.GetUpcomingEvents();

            SolarEvent nextEvent = upcomingSolarEvents.NextEvent;
            LightingStateChange nextChange = new LightingStateChange(
                nextEvent.Type == SolarEventType.Sunrise ? State.Off : State.On,
                nextEvent.Type == SolarEventType.Sunrise ? nextEvent.Timestamp.AddMinutes(-30) : nextEvent.Timestamp.AddMinutes(30),
                nextEvent);

            SolarEvent nextNextEvent = upcomingSolarEvents.NextNextEvent;
            LightingStateChange nextNextChange = new LightingStateChange(
                nextNextEvent.Type == SolarEventType.Sunrise ? State.Off : State.On,
                nextNextEvent.Type == SolarEventType.Sunrise ? nextNextEvent.Timestamp.AddMinutes(-30) : nextNextEvent.Timestamp.AddMinutes(30),
                nextNextEvent);

            return this.Ok(new UpcomingLightingStateChanges(nextChange, nextNextChange));
        }
    }
}
