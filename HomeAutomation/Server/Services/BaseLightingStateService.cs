using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;

namespace HomeAutomation.Server.Services
{
    public abstract class BaseLightingStateService : ILightingStateService
    {
        private readonly ISolarEventsService solarEventsService;

        public BaseLightingStateService(ISolarEventsService solarEventsService)
        {
            this.solarEventsService = solarEventsService;
        }

        public LightingStateChange GetNextChange()
        {
            SolarEvent firstSolarEvent = this.solarEventsService.GetFirstEvent();
            LightingStateChange nextChange = GetStateChangeFromSolarEvent(firstSolarEvent);
            if (nextChange.Timestamp <= DateTime.UtcNow)
            {
                SolarEvent secondSolarEvent = this.solarEventsService.GetSecondEvent();
                nextChange = GetStateChangeFromSolarEvent(secondSolarEvent);
            }

            return nextChange;
        }

        public UpcomingLightingStateChanges GetUpcomingLightingStateChanges()
        {
            LightingStateChange firstChange = this.GetNextChange();

            SolarEvent secondChangeSolarEvent;

            SolarEvent firstSolarEvent = this.solarEventsService.GetFirstEvent();
            if (firstChange.SolarEvent == firstSolarEvent)
            {
                secondChangeSolarEvent = this.solarEventsService.GetSecondEvent();
            }
            else
            {
                secondChangeSolarEvent = this.solarEventsService.GetThirdEvent();
            }

            LightingStateChange secondChange = GetStateChangeFromSolarEvent(secondChangeSolarEvent);

            return new UpcomingLightingStateChanges(firstChange, secondChange);
        }

        protected abstract DateTime GetStateChangeTimeForSolarEvent(SolarEvent solarEvent);

        private LightingStateChange GetStateChangeFromSolarEvent(SolarEvent solarEvent)
        {
            State state;
            DateTime timestamp;
            if (solarEvent.Type == SolarEventType.Sunrise)
            {
                state = State.Off;
                timestamp = this.GetStateChangeTimeForSolarEvent(solarEvent);
            }
            else
            {
                state = State.On;
                timestamp = this.GetStateChangeTimeForSolarEvent(solarEvent);
            }

            return new LightingStateChange(state, timestamp, solarEvent);
        }
    }
}
