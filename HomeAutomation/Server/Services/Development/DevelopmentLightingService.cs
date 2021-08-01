using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;

namespace HomeAutomation.Server.Services.Development
{
    public class DevelopmentLightingService : ILightingService
    {
        private State state = State.Off;

        public State GetState() => this.state;

        public void SetState(State newState) => this.state = newState;
    }
}
