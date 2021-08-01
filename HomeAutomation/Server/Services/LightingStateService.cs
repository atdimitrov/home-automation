using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;

namespace HomeAutomation.Server.Services
{
    public class LightingStateService : BaseLightingStateService, ILightingStateService
    {
        public LightingStateService(ISolarEventsService solarEventsService)
            : base(solarEventsService)
        {
        }

        protected override DateTime GetStateChangeTimeForSolarEvent(SolarEvent solarEvent)
        {
            if (solarEvent.Type == SolarEventType.Sunrise)
            {
                return solarEvent.Timestamp.AddMinutes(-30);
            }
            else
            {
                return solarEvent.Timestamp.AddMinutes(30);
            }
        }
    }
}
