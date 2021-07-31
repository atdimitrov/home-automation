namespace HomeAutomation.Shared
{
    public class UpcomingSolarEvents
    {
        public UpcomingSolarEvents(SolarEvent nextEvent, SolarEvent nextNextEvent)
        {
            this.NextEvent = nextEvent;
            this.NextNextEvent = nextNextEvent;
        }

        public SolarEvent NextEvent { get; }
        public SolarEvent NextNextEvent { get; }
    }
}
