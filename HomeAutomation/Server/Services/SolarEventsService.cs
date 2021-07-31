using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Innovative.SolarCalculator;
using System;

namespace HomeAutomation.Server.Services
{
    public class SolarEventsService : ISolarEventsService
    {
        public SolarEvent GetNextEvent()
        {
            DateTime now = DateTime.Now;
            SolarTimes solarTimesToday = new SolarTimes(now, 42.695537, 23.2539071);
            if (solarTimesToday.Sunrise > now)
            {
                return SolarEvent.Sunrise(solarTimesToday.Sunrise);
            }
            else if (solarTimesToday.Sunset > now)
            {
                return SolarEvent.Sunset(solarTimesToday.Sunset);
            }
            else
            {
                SolarTimes solarTimesTomorrow = new SolarTimes(now.AddDays(1), 42.695537, 23.2539071);
                return SolarEvent.Sunrise(solarTimesTomorrow.Sunrise);
            }
        }

        public UpcomingSolarEvents GetUpcomingEvents()
        {
            SolarEvent nextEvent = this.GetNextEvent();
            SolarEvent nextNextEvent;
            if (nextEvent.Type == SolarEventType.Sunrise)
            {
                SolarTimes solarTimes = new SolarTimes(nextEvent.Timestamp, 42.695537, 23.2539071);
                nextNextEvent = SolarEvent.Sunset(solarTimes.Sunset);
            }
            else
            {
                SolarTimes solarTimes = new SolarTimes(nextEvent.Timestamp.AddDays(1), 42.695537, 23.2539071);
                nextNextEvent = SolarEvent.Sunrise(solarTimes.Sunrise);
            }

            return new UpcomingSolarEvents(nextEvent, nextNextEvent);
        }
    }
}
