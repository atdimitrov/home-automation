using HomeAutomation.Shared;

namespace HomeAutomation.Server.Interfaces
{
    public interface ISolarEventsService
    {
        SolarEvent GetFirstEvent();
        
        SolarEvent GetSecondEvent();

        SolarEvent GetThirdEvent();
    }
}
