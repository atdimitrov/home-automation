using HomeAutomation.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeAutomation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemperatureController : ControllerBase
	{
        private readonly ILogger<TemperatureController> logger;
        private readonly ITemperatureService temperatureService;

        public TemperatureController(ILogger<TemperatureController> logger, ITemperatureService temperatureService)
        {
            this.logger = logger;
            this.temperatureService = temperatureService;
        }

        [HttpGet]
        public IActionResult Get()
		{
			this.logger.LogDebug("Received temperature reading request");

			Result<double> result = this.temperatureService.GetCurrentTemperature();
			if (result.IsSuccessful)
            {
				this.logger.LogDebug($"Successful temperature reading. Read value - {result.Value}");

				return this.Ok(result.Value);
			}
            else
            {
                this.logger.LogError(result.ErrorMessage);

                return this.StatusCode(500, result.ErrorMessage);
			}
		}
	}
}
