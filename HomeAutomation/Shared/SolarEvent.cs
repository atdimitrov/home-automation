using System;

namespace HomeAutomation.Shared
{
    public class SolarEvent : IEquatable<SolarEvent>
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

        public override string ToString() => $"{this.Type} at {this.Timestamp}";

        public override bool Equals(object obj) => this.Equals(obj as SolarEvent);

        public override int GetHashCode() => $"{this.Type}{this.Timestamp}".GetHashCode();

        public bool Equals(SolarEvent other) =>
            this.Type == other.Type && this.Timestamp == other.Timestamp;

        public static bool operator ==(SolarEvent a, SolarEvent b) => a.Equals(b);

        public static bool operator !=(SolarEvent a, SolarEvent b) => !a.Equals(b);
    }
}
