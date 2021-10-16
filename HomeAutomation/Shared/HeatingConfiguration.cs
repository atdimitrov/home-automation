using System;

namespace HomeAutomation.Shared
{
    public class HeatingConfiguration
    {
        public HeatingConfiguration(
            ControlMode controlMode,
            double targetTemperature,
            DateTime nightModeStartTime,
            DateTime nightModeEndTime,
            OperationMode operationMode)
        {
            this.ControlMode = controlMode;
            this.TargetTemperature = targetTemperature;
            this.NightModeStartTime = nightModeStartTime;
            this.NightModeEndTime = nightModeEndTime;
            this.OperationMode = operationMode;
        }

        public ControlMode ControlMode { get; }

        public double TargetTemperature { get; }

        public DateTime NightModeStartTime { get; }

        public DateTime NightModeEndTime { get; }

        public OperationMode OperationMode { get; }
    }
}
