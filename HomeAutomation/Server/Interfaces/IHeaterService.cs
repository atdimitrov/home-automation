using HomeAutomation.Shared;
using System.Collections.Generic;

namespace HomeAutomation.Server.Interfaces
{
    public interface IHeaterService
    {
        List<Heater> GetHeaters();

        void ToggleHeater(Heater heater);
    }
}
