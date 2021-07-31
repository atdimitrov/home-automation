using HomeAutomation.Server.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace HomeAutomation.Server.Services
{
    public class TemperatureService : ITemperatureService
    {
        private readonly ILogger<TemperatureService> logger;

        public TemperatureService(ILogger<TemperatureService> logger)
        {
            this.logger = logger;
        }

        public Result<double> GetCurrentTemperature()
        {
			/*
			 * File format
			 * c1 01 ff ff 7f ff ff ff 6e : crc=6e YES
			 * c1 01 ff ff 7f ff ff ff 6e t=28062
			 */

			string sensorReading = File.ReadAllText("/sys/bus/w1/devices/28-8000001fa053/w1_slave");
			this.logger.LogDebug($"Sensor reading:{Environment.NewLine}{sensorReading}");

			string[] lines = sensorReading.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			if (lines.Length != 2)
			{
				string error = $"Temperature reading failed - expected 2 lines in file, found {lines.Length}.";
				return Result<double>.Failure(error);
			}

			if (!lines[0].EndsWith("YES"))
			{
				string error = "Temperature reading failed - sensor reported failure.";
				return Result<double>.Failure(error);
			}

			int indexOfTemperatureReading = lines[1].IndexOf("t=");
			if (indexOfTemperatureReading == -1)
			{
				string error = "Temperature reading failed - \"t=\" is missing.";
				return Result<double>.Failure(error);
			}

			string temperatureReading = lines[1].Substring(indexOfTemperatureReading + 2);
			if (!int.TryParse(temperatureReading, out int millidegrees))
			{
				string error = $"Temperature reading failed - failed to parse temperature reading \"{temperatureReading}\".";
				return Result<double>.Failure(error);
			}

			double degrees = Math.Round(millidegrees / 1000d, 1);
			return Result<double>.Success(degrees);
		}
    }
}
