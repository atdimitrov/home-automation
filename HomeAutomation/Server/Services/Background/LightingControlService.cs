using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HomeAutomation.Server.Services.Background
{
    public class LightingControlService : IHostedService, IDisposable
    {
        private readonly ILogger<LightingControlService> logger;
        private readonly ILightingStateService lightingStateService;
        private readonly ILightingService lightingService;

        private bool disposed;
        private System.Timers.Timer timer;
        private LightingStateChange nextChange;

        public LightingControlService(
            ILogger<LightingControlService> logger,
            ILightingStateService lightingStateService,
            ILightingService lightingService
        )
        {
            this.logger = logger;
            this.lightingStateService = lightingStateService;
            this.lightingService = lightingService;

            this.timer = new System.Timers.Timer();
            this.timer.AutoReset = false;
            this.timer.Elapsed += this.OnElapsed;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.timer.Dispose();

            this.disposed = true;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LightingStateChange nextChange = this.lightingStateService.GetNextChange();
            if (nextChange.NewState == State.Off)
            {
                this.logger.LogInformation($"Turning lighting on as preparation for upcoming {nextChange.SolarEvent}");

                this.lightingService.SetState(State.On);
            }

            this.ScheduleNextChange(nextChange);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer.Stop();

            return Task.CompletedTask;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            this.logger.LogInformation($"Turning lighting {this.nextChange.NewState} for {this.nextChange.SolarEvent}");

            this.lightingService.SetState(this.nextChange.NewState);

            this.ScheduleNextChange(this.lightingStateService.GetNextChange());
        }

        private void ScheduleNextChange(LightingStateChange nextChange)
        {
            this.logger.LogInformation($"Scheduling lighting to turn {nextChange.NewState} at {nextChange.Timestamp} for {nextChange.SolarEvent}");

            this.nextChange = nextChange;

            this.timer.Interval = (nextChange.Timestamp - DateTime.UtcNow).TotalMilliseconds;
            this.timer.Start();
        }
    }
}
