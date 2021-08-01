using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Innovative.SolarCalculator;
using System;

namespace HomeAutomation.Server.Services
{
    public class SolarEventsService : ISolarEventsService
    {
        public SolarEvent GetFirstEvent()
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

        public SolarEvent GetSecondEvent() => GetNextEvent(this.GetFirstEvent());

        public SolarEvent GetThirdEvent() => GetNextEvent(this.GetSecondEvent());

        private static SolarEvent GetNextEvent(SolarEvent solarEvent)
        {
            if (solarEvent.Type == SolarEventType.Sunrise)
            {
                SolarTimes solarTimes = new SolarTimes(solarEvent.Timestamp, 42.695537, 23.2539071);
                return SolarEvent.Sunset(solarTimes.Sunset);
            }
            else
            {
                SolarTimes solarTimes = new SolarTimes(solarEvent.Timestamp.AddDays(1), 42.695537, 23.2539071);
                return SolarEvent.Sunrise(solarTimes.Sunrise);
            }
        }
    }
}
