using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;

namespace HomeAutomation.Server.Services.Development
{
    public class DevelopmentLightingStateService : BaseLightingStateService, ILightingStateService
    {
        public DevelopmentLightingStateService(ISolarEventsService solarEventsService)
            : base(solarEventsService)
        {
        }

        protected override DateTime GetStateChangeTimeForSolarEvent(SolarEvent solarEvent)
        {
            if (solarEvent.Type == SolarEventType.Sunrise)
            {
                return solarEvent.Timestamp.AddSeconds(-10);
            }
            else
            {
                return solarEvent.Timestamp.AddSeconds(10);
            }
        }
    }
}
