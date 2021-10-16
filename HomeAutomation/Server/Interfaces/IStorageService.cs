using System;
using System.Threading.Tasks;

namespace HomeAutomation.Server.Interfaces
{
    public interface IStorageService
    {
        Task<bool> HasKey(string key);

        Task<string> ReadString(string key);

        Task<double> ReadDouble(string key);

        Task<T> ReadEnum<T>(string key) where T : struct;

        Task<DateTime> ReadDateTime(string key);

        Task WriteString(string key, string value);

        Task WriteDouble(string key, double value);

        Task WriteEnum<T>(string key, T value) where T : struct;

        Task WriteDateTime(string key, DateTime value);
    }
}
