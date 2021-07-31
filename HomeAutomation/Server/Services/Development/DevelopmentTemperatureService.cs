using HomeAutomation.Server.Interfaces;
using System;

namespace HomeAutomation.Server.Services.Development
{
    public class DevelopmentTemperatureService : ITemperatureService
    {
        private readonly Random random = new Random();

        public Result<double> GetCurrentTemperature()
        {
            return Result<double>.Success(Math.Round(random.Next(18, 30) + random.NextDouble(), 1));
        }
    }
}
