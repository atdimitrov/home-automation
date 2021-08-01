﻿using HomeAutomation.Shared;

namespace HomeAutomation.Server
{
    public class UpcomingLightingStateChanges
    {
        public UpcomingLightingStateChanges(LightingStateChange firstChange, LightingStateChange secondChange)
        {
            this.FirstChange = firstChange;
            this.SecondChange = secondChange;
        }

        public LightingStateChange FirstChange { get; }
        public LightingStateChange SecondChange { get; }
    }
}
