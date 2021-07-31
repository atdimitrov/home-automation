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
        private readonly ISolarEventsService solarEventsService;
        private readonly ILightingService lightingService;

        private bool disposed;
        private System.Timers.Timer timer;
        private SolarEvent nextEvent;

        public LightingControlService(
            ILogger<LightingControlService> logger,
            ISolarEventsService solarEventsService,
            ILightingService lightingService
        )
        {
            this.logger = logger;
            this.solarEventsService = solarEventsService;
            this.lightingService = lightingService;

            this.timer = new System.Timers.Timer();
            this.timer.AutoReset = false;
            this.timer.Elapsed += OnElapsed;
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
            SolarEvent nextEvent = this.solarEventsService.GetNextEvent();
            if (nextEvent.Type == SolarEventType.Sunrise)
            {
                this.logger.LogInformation("Initialized after sunset, turning lighting on");

                this.lightingService.TurnOn();
            }

            this.ScheduleNextEvent(nextEvent);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer.Stop();

            return Task.CompletedTask;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            string lightingOperation = this.nextEvent.Type == SolarEventType.Sunrise ? "off" : "on";
            this.logger.LogInformation($"Turning lighting {lightingOperation} for {this.nextEvent.Type} at {this.nextEvent.Timestamp}");

            if (this.nextEvent.Type == SolarEventType.Sunrise)
            {
                this.lightingService.TurnOff();
            }
            else
            {
                this.lightingService.TurnOn();
            }

            this.ScheduleNextEvent(this.solarEventsService.GetNextEvent());
        }

        private void ScheduleNextEvent(SolarEvent nextEvent)
        {
            DateTime lightingOperationTimestamp = GetLightingOperationTimestampForSolarEvent(nextEvent);

            string nextEventLightingOperation = nextEvent.Type == SolarEventType.Sunrise ? "off" : "on";
            this.logger.LogInformation($"Scheduling lighting to turn {nextEventLightingOperation} at {lightingOperationTimestamp} for {nextEvent.Type} at {nextEvent.Timestamp}");

            this.nextEvent = nextEvent;

            this.timer.Interval = (lightingOperationTimestamp - DateTime.Now).TotalMilliseconds;
            this.timer.Start();
        }

        private static DateTime GetLightingOperationTimestampForSolarEvent(SolarEvent solarEvent)
        {
            int minuteOffset = 30;
            if (solarEvent.Type == SolarEventType.Sunrise)
            {
                minuteOffset *= -1;
            }

            return solarEvent.Timestamp.AddMinutes(minuteOffset);
        }
    }
}
