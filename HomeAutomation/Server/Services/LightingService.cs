using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;

namespace HomeAutomation.Server.Services
{
    public class LightingService : BaseGpioService, ILightingService
    {
        private static readonly int LightingPin = 26;

        public LightingService()
            : base(LightingPin)
        {
        }

        public State GetState() => this.GetPinState(LightingPin);

        public void SetState(State newState) => this.SetPinState(LightingPin, newState);
    }
}
