using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;

namespace HomeAutomation.Server.Services.Development
{
    public class DevelopmentLightingService : ILightingService
    {
        private State state = State.Off;

        public State GetState() => this.state;

        public void TurnOn() => this.state = State.On;
        
        public void TurnOff() => this.state = State.Off;
    }
}
