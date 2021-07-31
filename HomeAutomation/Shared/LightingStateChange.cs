using System;

namespace HomeAutomation.Shared
{
    public class LightingStateChange
    {
        public LightingStateChange(State newState, DateTime timestamp, SolarEvent solarEvent)
        {
            this.NewState = newState;
            this.Timestamp = timestamp;
            this.SolarEvent = solarEvent;
        }

        public State NewState { get; }
        public DateTime Timestamp { get; }
        public SolarEvent SolarEvent { get; }
    }
}