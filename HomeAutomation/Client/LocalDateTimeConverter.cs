using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.Client
{
    public class LocalDateTimeConverter
    {
        private readonly IJSRuntime js;
        private int timezoneOffset;
        private bool initialized;

        public LocalDateTimeConverter(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task Initialize()
        {
            if (this.initialized)
                return;

            this.timezoneOffset = await this.js.InvokeAsync<int>("getClientTimezoneOffset");

            this.initialized = true;
        }

        public DateTime ConvertToLocalDateTime(DateTime utc)
        {
            if (!initialized)
                throw new InvalidOperationException($"{nameof(LocalDateTimeConverter)} should be initialized with {nameof(Initialize)}() before usage.");

            return utc.Add(TimeSpan.FromMinutes(-timezoneOffset));
        }
    }
}
