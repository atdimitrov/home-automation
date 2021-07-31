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

        public void TurnOn() => this.SetPinState(LightingPin, State.On);

        public void TurnOff() => this.SetPinState(LightingPin, State.Off);
    }
}
