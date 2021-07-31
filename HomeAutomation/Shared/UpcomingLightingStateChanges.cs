namespace HomeAutomation.Shared
{
    public class UpcomingLightingStateChanges
    {
        public UpcomingLightingStateChanges(LightingStateChange nextChange, LightingStateChange nextNextChange)
        {
            this.NextChange = nextChange;
            this.NextNextChange = nextNextChange;
        }

        public LightingStateChange NextChange { get; }
        public LightingStateChange NextNextChange { get; }
    }
}
