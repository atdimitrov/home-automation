using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.Server.Services
{
    public class HeatingLogicService : IHeatingLogicService
    {
        private readonly ITemperatureService temperatureService;
        private readonly IHeatingService heatingService;
        private readonly IHeaterService heaterService;

        public HeatingLogicService(ITemperatureService temperatureService, IHeatingService heatingService, IHeaterService heaterService)
        {
            this.temperatureService = temperatureService;
            this.heatingService = heatingService;
            this.heaterService = heaterService;
        }

        /// <summary>
        /// Simple state machine.
        /// 
        /// Example:
        ///     Target temperature - 26
        ///     
        ///     25.5    26    26.5    27        28
        ///      <=            >=     <=        >=
        ///     2 ON          2 OFF   1 ON      1 OFF
        ///     
        ///     dT - temperature difference (Actual - Target)
        ///
        ///          dT <= 1         dT <= -0.5
        ///      /------>-------\  /------>------\
        ///     0                1                2
        ///      \------<-------/  \------<------/
        ///          dT >= 2          dT >= 0.5
        /// </summary>
        public async Task<Result<uint>> GetTargetNumberOfHeaters()
        {
            Result<double> getCurrentTemperatureResult = this.temperatureService.GetCurrentTemperature();
            if (!getCurrentTemperatureResult.IsSuccessful)
            {
                return Result<uint>.Failure(getCurrentTemperatureResult.ErrorMessage);
            }

            double currentTemperature = getCurrentTemperatureResult.Value;
            double targetTemperature = await this.heatingService.GetTargetTemperature();
            double dT = currentTemperature - targetTemperature;

            uint numberOfRunningHeaters = (uint)this.heaterService.GetHeaters().Count(h => h.State == State.On);
            bool hasStateChange;
            do
            {
                hasStateChange = false;

                switch (numberOfRunningHeaters)
                {
                    case 0:
                        if (dT <= 1)
                        {
                            numberOfRunningHeaters = 1;
                            hasStateChange = true;
                        }
                        break;
                    case 1:
                        if (dT >= 2)
                        {
                            numberOfRunningHeaters = 0;
                            hasStateChange = true;
                        }
                        else if (dT <= -0.5)
                        {
                            numberOfRunningHeaters = 2;
                            hasStateChange = true;
                        }
                        break;
                    case 2:
                        if (dT >= 0.5)
                        {
                            numberOfRunningHeaters = 1;
                            hasStateChange = true;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            } while (hasStateChange);
            
            if (await this.heatingService.GetOperationMode() == OperationMode.Night && numberOfRunningHeaters < 2)
            {
                numberOfRunningHeaters++;
            }

            return Result<uint>.Success(numberOfRunningHeaters);
        }
    }
}
