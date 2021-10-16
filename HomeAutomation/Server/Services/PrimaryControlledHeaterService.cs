using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System.Collections.Generic;
using System.Linq;

namespace HomeAutomation.Server.Services
{
    public class PrimaryControlledHeaterService : IHeaterService
    {
        private readonly IHeaterService heaterService;

        public PrimaryControlledHeaterService(IHeaterService heaterService)
        {
            this.heaterService = heaterService;
        }

        public List<Heater> GetHeaters()
        {
            return this.heaterService.GetHeaters();
        }

        public void ToggleHeater(Heater heater)
        {
            // Turning off primary heater requires all other heaters to be turned off as well
            if (heater.Primary && heater.State == State.On)
            {
                IEnumerable<Heater> turnedOnSecondaryHeaters = this.GetHeaters().Where(h => !h.Primary && h.State == State.On);
                foreach (Heater secondaryHeater in turnedOnSecondaryHeaters)
                {
                    this.heaterService.ToggleHeater(secondaryHeater);
                }
            }
            // Turning on secondary heater requires the primary heater to be turned on as well
            else if (!heater.Primary && heater.State == State.Off)
            {
                Heater primaryHeater = this.GetHeaters().First(h => h.Primary);
                if (primaryHeater.State == State.Off)
                {
                    this.heaterService.ToggleHeater(primaryHeater);
                }
            }

            this.heaterService.ToggleHeater(heater);
        }
    }
}
