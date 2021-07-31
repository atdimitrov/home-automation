using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System.Collections.Generic;
using System.Linq;

namespace HomeAutomation.Server.Services.Development
{
    public class DevelopmentHeaterService : IHeaterService
    {
        private readonly Dictionary<int, State> heaters;

        public DevelopmentHeaterService()
        {
            this.heaters = new Dictionary<int, State>()
            {
                { 0, State.Off },
                { 1, State.Off }
            };
        }

        public List<Heater> GetHeaters()
        {
            return this.heaters.Select(pair => new Heater(pair.Key, pair.Value)).ToList();
        }

        public void ToggleHeater(Heater heater)
        {
            State newState;
            if (heater.State == State.On)
            {
                newState = State.Off;
            }
            else
            {
                newState = State.On;
            }

            this.heaters[heater.Id] = newState;
        }
    }
}
