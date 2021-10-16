using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HeatingController : ControllerBase
    {
        private readonly IHeatingService heatingService;

        public HeatingController(IHeatingService heatingService)
        {
            this.heatingService = heatingService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ControlMode mode = await this.heatingService.GetControlMode();
            double targetTemperature = await this.heatingService.GetTargetTemperature();
            DateTime nightModeStartTime = await this.heatingService.GetNightModeStartTime();
            DateTime nightModeEndTime = await this.heatingService.GetNightModeEndTime();
            OperationMode operationMode = await this.heatingService.GetOperationMode();

            return this.Ok(new HeatingConfiguration(mode, targetTemperature, nightModeStartTime, nightModeEndTime, operationMode));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleControlMode()
        {
            ControlMode mode = await this.heatingService.GetControlMode();
            if (mode == ControlMode.Automatic)
                await this.heatingService.SetControlMode(ControlMode.Manual);
            else
                await this.heatingService.SetControlMode(ControlMode.Automatic);

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SetTargetTemperature([FromBody] double newTarget)
        {
            if (newTarget < 20 || newTarget > 30)
            {
                return ValidationProblem($"Target temperature must be between 20 and 30 degrees Celsius, inclusive. Provided value - {newTarget}.");
            }

            await this.heatingService.SetTargetTemperature(newTarget);

            return this.Ok();
        }
    }
}
