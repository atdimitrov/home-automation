using HomeAutomation.Server.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.Server.Services
{
    public class RedisStorageService : IStorageService
    {
        private readonly string url;
        private ConnectionMultiplexer connection;

        public RedisStorageService(IConfiguration configuration)
        {
            this.url = configuration["RedisUrl"];
        }

        public async Task<bool> HasKey(string key)
        {
            await this.EnsureInitialized();

            IDatabase db = connection.GetDatabase(0);
            return db.KeyExists(key);
        }

        public async Task<double> ReadDouble(string key)
        {
            await this.EnsureInitialized();
            return double.Parse(await this.ReadStringInternal(key));
        }

        public async Task<string> ReadString(string key)
        {
            await this.EnsureInitialized();
            return await this.ReadStringInternal(key);
        }

        public async Task<T> ReadEnum<T>(string key)
            where T : struct
        {
            await this.EnsureInitialized();
            return Enum.Parse<T>(await this.ReadString(key));
        }

        public async Task<DateTime> ReadDateTime(string key)
        {
            await this.EnsureInitialized();
            return DateTime.Parse(await this.ReadStringInternal(key));
        }

        public async Task WriteDouble(string key, double value)
        {
            await this.EnsureInitialized();
            await this.WriteStringInternal(key, value.ToString());
        }

        public async Task WriteString(string key, string value)
        {
            await this.EnsureInitialized();
            await this.WriteStringInternal(key, value);
        }

        public async Task WriteEnum<T>(string key, T value)
            where T : struct
        {
            await this.EnsureInitialized();
            await this.WriteStringInternal(key, value.ToString());
        }

        public async Task WriteDateTime(string key, DateTime value)
        {
            await this.EnsureInitialized();
            await this.WriteStringInternal(key, value.ToString("s"));
        }

        private async Task EnsureInitialized()
        {
            if (this.connection == null)
                this.connection = await ConnectionMultiplexer.ConnectAsync(this.url);
        }

        private async Task<string> ReadStringInternal(string key)
        {
            IDatabase db = connection.GetDatabase(0);
            return await db.StringGetAsync(key);
        }

        private async Task WriteStringInternal(string key, string value)
        {
            IDatabase db = connection.GetDatabase(0);
            await db.StringSetAsync(key, value);
        }
    }
}
