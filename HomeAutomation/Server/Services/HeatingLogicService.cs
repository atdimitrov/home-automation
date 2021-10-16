using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.Server.Services
{
    public class HeatingLogicService : IHeatingLogicService
    {
        private readonly ITemperatureService temperatureService;
        private readonly IHeatingService heatingService;

        private uint targetNumberOfRunningHeaters = 0;

        public HeatingLogicService(ITemperatureService temperatureService, IHeatingService heatingService)
        {
            this.temperatureService = temperatureService;
            this.heatingService = heatingService;
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

            switch (this.targetNumberOfRunningHeaters)
            {
                case 0:
                    if (dT <= 1)
                        this.targetNumberOfRunningHeaters = 1;
                    break;
                case 1:
                    if (dT >= 2)
                        this.targetNumberOfRunningHeaters = 0;
                    else if (dT <= -0.5)
                        this.targetNumberOfRunningHeaters = 2;
                    break;
                case 2:
                    if (dT >= 0.5)
                        this.targetNumberOfRunningHeaters = 1;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (await this.heatingService.GetOperationMode() == OperationMode.Night &&
                this.targetNumberOfRunningHeaters == 1)
            {
                this.targetNumberOfRunningHeaters = 2;
            }

            return Result<uint>.Success(this.targetNumberOfRunningHeaters);
        }
    }
}
