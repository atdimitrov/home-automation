using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeAutomation.Server.Services.Development
{
    public class DevelopmentSolarEventsService : ISolarEventsService
    {
        private readonly List<SolarEvent> solarEvents;

        public DevelopmentSolarEventsService()
        {
            this.solarEvents = new List<SolarEvent>();

            SolarEventType nextEventType = SolarEventType.Sunrise;
            DateTime nextEventTimestamp = DateTime.UtcNow.AddMinutes(1);

            for (int i = 0; i < 1000; i++)
            {
                this.solarEvents.Add(new SolarEvent(nextEventType, nextEventTimestamp));

                nextEventType = nextEventType == SolarEventType.Sunrise ? SolarEventType.Sunset : SolarEventType.Sunrise;
                nextEventTimestamp = nextEventTimestamp.AddMinutes(1);
            }
        }

        public SolarEvent GetFirstEvent() => this.GeUpcomingEvent(1);

        public SolarEvent GetSecondEvent() => this.GeUpcomingEvent(2);

        public SolarEvent GetThirdEvent() => this.GeUpcomingEvent(3);

        private SolarEvent GeUpcomingEvent(int sequenceNumber) =>
            this.solarEvents.Where(e => e.Timestamp > DateTime.UtcNow).Skip(sequenceNumber - 1).First();
    }
}
