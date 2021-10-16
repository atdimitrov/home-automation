using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HomeAutomation.Server.Services.Background
{
    public class AutomaticHeatingControlService : IHostedService, IDisposable
    {
        private const double CheckInterval = 5 * 60 * 1000;

        private readonly ILogger<AutomaticHeatingControlService> logger;
        private readonly IHeatingService heatingService;
        private readonly IHeatingLogicService heatingLogic;
        private readonly IHeaterService heaterService;
        private readonly System.Timers.Timer timer;

        private bool disposed;

        public AutomaticHeatingControlService(
            ILogger<AutomaticHeatingControlService> logger,
            IHeatingService heatingService,
            IHeatingLogicService heatingLogic,
            IHeaterService heaterService
        )
        {
            this.logger = logger;
            this.heatingService = heatingService;
            this.heatingLogic = heatingLogic;
            this.heaterService = heaterService;

            this.heatingService.ControlModeChanged += this.OnControlModeChanged;
            this.heatingService.TargetTemperatureChanged += this.OnTargetTemperatureChanged;

            this.timer = new System.Timers.Timer();
            this.timer.Interval = CheckInterval;
            this.timer.Elapsed += this.OnElapsed;
        }

        public void Dispose()
        {
            if (this.disposed)
                return;

            this.timer.Dispose();
            this.heatingService.ControlModeChanged -= this.OnControlModeChanged;
            this.heatingService.TargetTemperatureChanged -= this.OnTargetTemperatureChanged;

            this.disposed = true;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ControlMode mode = await this.heatingService.GetControlMode();
            if (mode == ControlMode.Automatic)
            {
                await this.StartAutomaticControl();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer.Stop();

            return Task.CompletedTask;
        }

        private async void OnControlModeChanged(object sender, ControlMode newMode)
        {
            if (newMode == ControlMode.Automatic)
            {
                await this.StartAutomaticControl();
            }
            else
            {
                this.timer.Stop();
            }
        }

        private async void OnTargetTemperatureChanged(object sender, double e)
        {
            if (await this.heatingService.GetControlMode() == ControlMode.Automatic)
            {
                await this.CheckStateMachine();
            }
        }

        private async Task StartAutomaticControl()
        {
            this.logger.LogInformation("Starting automatic heating control.");

            await this.CheckStateMachine();

            this.timer.Start();
        }

        private async void OnElapsed(object sender, ElapsedEventArgs e)
        {
            await this.CheckStateMachine();
        }

        private async Task CheckStateMachine()
        {
            Result<uint> getTargetNumberOfHeatersResult = await this.heatingLogic.GetTargetNumberOfHeaters();
            if (!getTargetNumberOfHeatersResult.IsSuccessful)
            {
                this.logger.LogError($"Failed to get target number of heaters. Error message: \"{getTargetNumberOfHeatersResult.ErrorMessage}\".");

                return;
            }

            int targetNumberOfHeaters = (int)getTargetNumberOfHeatersResult.Value;
            List<Heater> heaters = this.heaterService.GetHeaters();

            for (int i = 0; i < targetNumberOfHeaters; i++)
            {
                if (heaters[i].State == State.Off)
                {
                    this.heaterService.ToggleHeater(heaters[i]);
                }
            }

            for (int i = targetNumberOfHeaters; i < heaters.Count; i++)
            {
                if (heaters[i].State == State.On)
                {
                    this.heaterService.ToggleHeater(heaters[i]);
                }
            }
        }
    }
}
