using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.Server.Services
{
    public class HeatingService : IHeatingService
    {
        private const string ControlModeKey = "CONTROL_MODE";
        private const string TargetTemperatureKey = "TARGET_TEMPERATURE";
        private const string NightModeStartTimeKey = "NIGHT_MODE_START_TIME";
        private const string NightModeEndTimeKey = "NIGHT_MODE_END_TIME";

        private const ControlMode DefaultControlMode = ControlMode.Manual;
        private const double DefaultTargetTemperature = 28;
        private readonly DateTime DefaultNightModeStartTime = new DateTime(1970, 1, 1, 20, 0, 0, DateTimeKind.Utc);
        private readonly DateTime DefaultNightModeEndTime = new DateTime(1970, 1, 1, 4, 0, 0, DateTimeKind.Utc);

        public event EventHandler<ControlMode> ControlModeChanged;
        public event EventHandler<double> TargetTemperatureChanged;
        public event EventHandler<DateTime> NightModeStartTimeChanged;
        public event EventHandler<DateTime> NightModeEndTimeChanged;
        public event EventHandler<OperationMode> OperationModeChanged;

        private readonly IStorageService storageService;

        public HeatingService(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public async Task<ControlMode> GetControlMode()
        {
            if (!await this.storageService.HasKey(ControlModeKey))
                await this.SetControlMode(DefaultControlMode);

            return await this.storageService.ReadEnum<ControlMode>(ControlModeKey);
        }

        public async Task<double> GetTargetTemperature()
        {
            if (!await this.storageService.HasKey(TargetTemperatureKey))
                await SetTargetTemperature(DefaultTargetTemperature);

            return await this.storageService.ReadDouble(TargetTemperatureKey);
        }

        public async Task<DateTime> GetNightModeStartTime()
        {
            // Bypass the operation mode change check if there is no value by using
            // the internal method. Otherwise it results in endless recursion.
            if (!await this.storageService.HasKey(NightModeStartTimeKey))
                await SetNightModeStartTimeInternal(DefaultNightModeStartTime);

            return await this.storageService.ReadDateTime(NightModeStartTimeKey);
        }

        public async Task<DateTime> GetNightModeEndTime()
        {
            // Bypass the operation mode change check if there is no value by using
            // the internal method. Otherwise it results in endless recursion.
            if (!await this.storageService.HasKey(NightModeEndTimeKey))
                await SetNightModeEndTimeInternal(DefaultNightModeEndTime);

            return await this.storageService.ReadDateTime(NightModeEndTimeKey);
        }

        public async Task<OperationMode> GetOperationMode()
        {
            DateTime nightModeStartTime = await this.GetNightModeStartTime();
            DateTime nightModeEndTime = await this.GetNightModeEndTime();

            if (DateTime.UtcNow.IsTimeAfter(nightModeStartTime) ||
                DateTime.UtcNow.IsTimeBefore(nightModeEndTime))
            {
                return OperationMode.Night;
            }
            else
            {
                return OperationMode.Day;
            }
        }

        public async Task SetControlMode(ControlMode newMode)
        {
            await this.storageService.WriteEnum(ControlModeKey, newMode);

            this.ControlModeChanged?.Invoke(this, newMode);
        }

        public async Task SetTargetTemperature(double newTarget)
        {
            await this.storageService.WriteDouble(TargetTemperatureKey, newTarget);

            this.TargetTemperatureChanged?.Invoke(this, newTarget);
        }

        public async Task SetNightModeStartTime(DateTime newStartTime)
        {
            await this.CheckForOperationModeChange(async () =>
            {
                await this.SetNightModeStartTimeInternal(newStartTime);
            });
        }

        public async Task SetNightModeEndTime(DateTime newEndTime)
        {
            await this.CheckForOperationModeChange(async () =>
            {
                await this.SetNightModeEndTimeInternal(newEndTime);
            });
        }

        private async Task SetNightModeStartTimeInternal(DateTime newStartTime)
        {
            await this.storageService.WriteDateTime(NightModeStartTimeKey, newStartTime);

            this.NightModeStartTimeChanged?.Invoke(this, newStartTime);
        }

        private async Task SetNightModeEndTimeInternal(DateTime newEndTime)
        {
            await this.storageService.WriteDateTime(NightModeEndTimeKey, newEndTime);

            this.NightModeEndTimeChanged?.Invoke(this, newEndTime);
        }

        private async Task CheckForOperationModeChange(Func<Task> action)
        {
            OperationMode oldOperationMode = await this.GetOperationMode();
            await action();
            OperationMode newOperationMode = await this.GetOperationMode();
            if (newOperationMode != oldOperationMode)
                this.OperationModeChanged?.Invoke(this, newOperationMode);
        }
    }
}
