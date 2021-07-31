using HomeAutomation.Server.Interfaces;
using HomeAutomation.Shared;
using System;
using System.Collections.Generic;

namespace HomeAutomation.Server.Services
{
    public class HeaterService : BaseGpioService, IHeaterService
    {
        private static readonly int[] HeatersPins = new int[] { 20, 21 };

        public HeaterService()
            : base(HeatersPins)
        {
        }

        public List<Heater> GetHeaters()
        {
            List<Heater> heaters = new List<Heater>();

            for (int i = 0; i < HeatersPins.Length; i++)
            {
                State state = GetPinState(HeatersPins[i]);
                heaters.Add(new Heater(i, state));
            }

            return heaters;
        }

        public void ToggleHeater(Heater heater)
        {
            State oldState = this.GetPinState(HeatersPins[heater.Id]);
            if (heater.State != oldState)
            {
                throw new ArgumentException($"The provided heater has invalid state. Provided state: {heater.State}; actual state: {oldState}.");
            }

            State newState;
            if (oldState == State.On)
            {
                newState = State.Off;
            }
            else
            {
                newState = State.On;
            }

            this.SetPinState(HeatersPins[heater.Id], newState);
        }
    }
}
