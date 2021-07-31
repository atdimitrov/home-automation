namespace HomeAutomation.Server.Interfaces
{
    public interface ITemperatureService
    {
        Result<double> GetCurrentTemperature();
    }
}
