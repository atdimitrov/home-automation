using HomeAutomation.Shared;

namespace HomeAutomation.Server.Interfaces
{
    public interface ILightingStateService
    {
        LightingStateChange GetNextChange();

        UpcomingLightingStateChanges GetUpcomingLightingStateChanges();
    }
}
