using System;
using System.Threading.Tasks;
using HomeAutomation.Shared;

namespace HomeAutomation.Server.Interfaces
{
    public interface IHeatingService
    {
        event EventHandler<ControlMode> ControlModeChanged;

        event EventHandler<double> TargetTemperatureChanged;

        event EventHandler<DateTime> NightModeStartTimeChanged;

        event EventHandler<DateTime> NightModeEndTimeChanged;

        event EventHandler<OperationMode> OperationModeChanged;

        Task<ControlMode> GetControlMode();

        Task<double> GetTargetTemperature();

        Task<DateTime> GetNightModeStartTime();

        Task<DateTime> GetNightModeEndTime();

        Task<OperationMode> GetOperationMode();

        Task SetControlMode(ControlMode mode);

        Task SetTargetTemperature(double temperature);

        Task SetNightModeStartTime(DateTime startTime);

        Task SetNightModeEndTime(DateTime endTime);
    }
}
