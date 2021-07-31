using System;

namespace HomeAutomation.Shared
{
    public class SolarEvent
    {
        public SolarEvent(SolarEventType type, DateTime timestamp)
        {
            this.Type = type;
            this.Timestamp = timestamp;
        }

        public static SolarEvent Sunrise(DateTime timestamp)
            => new SolarEvent(SolarEventType.Sunrise, timestamp);

        public static SolarEvent Sunset(DateTime timestamp)
            => new SolarEvent(SolarEventType.Sunset, timestamp);

        public SolarEventType Type { get; }
        public DateTime Timestamp { get; }
    }
}
