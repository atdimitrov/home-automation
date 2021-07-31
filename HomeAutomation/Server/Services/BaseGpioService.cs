using HomeAutomation.Shared;
using System;
using System.Device.Gpio;

namespace HomeAutomation.Server.Services
{
    public abstract class BaseGpioService : IDisposable
    {
        private readonly GpioController controller;

        private bool disposed = false;

        public BaseGpioService(params int[] outputPins)
        {
            this.controller = new GpioController();

            foreach (int pin in outputPins)
            {
                this.controller.OpenPin(pin, PinMode.Output, PinValue.High);
            }
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.controller.Dispose();

            this.disposed = true;
        }

        protected State GetPinState(int pin)
        {
            PinValue value = this.controller.Read(pin);
            if (value == PinValue.High)
            {
                return State.Off;
            }
            else
            {
                return State.On;
            }
        }

        protected void SetPinState(int pin, State newState)
        {
            PinValue newValue;
            if (newState == State.On)
            {
                newValue = PinValue.Low;
            }
            else
            {
                newValue = PinValue.High;
            }

            this.controller.Write(pin, newValue);
        }
    }
}
