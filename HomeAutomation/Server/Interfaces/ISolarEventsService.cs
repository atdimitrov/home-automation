using HomeAutomation.Shared;

namespace HomeAutomation.Server.Interfaces
{
    public interface ISolarEventsService
    {
        SolarEvent GetNextEvent();

        UpcomingSolarEvents GetUpcomingEvents();
    }
}
