using HomeAutomation.Shared;

namespace HomeAutomation.Server.Interfaces
{
    public interface ILightingService
    {
        State GetState();

        void TurnOn();

        void TurnOff();
    }
}
