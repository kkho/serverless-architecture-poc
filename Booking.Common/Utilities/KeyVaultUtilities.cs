using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Booking.Common.Utilities
{
    public sealed class KeyVaultUtility
    {
        public string KeyVaultUrl { get; set; }
        public TimeSpan DefaultTimeToLive { get; set; }

        public KeyVaultUtility(string keyVaultUrl) : this(keyVaultUrl, TimeSpan.FromDays(1)) => this.KeyVaultUrl = KeyVaultUrl;

        public KeyVaultUtility(string keyVaultUrl, TimeSpan defaultTimeToLive)
        {
            this.KeyVaultUrl = KeyVaultUrl;
            this.DefaultTimeToLive = defaultTimeToLive;
        }

        public Task<string> this[string value, bool update = false] => GetCachedSecret(value, this.KeyVaultUrl + value, DateTime.UtcNow.Add(DefaultTimeToLive), update);

        public async Task<string> GetSecret(string keyName) => await GetSecret(KeyVaultUrl, keyName);

        public static async Task<string> GetSecret(string baseUrl, string keyName)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

            using (var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)))
            {
                var secret = await keyVaultClient.GetSecretAsync(baseUrl, keyName).ConfigureAwait(false);

                string s = secret.Value;

                return s;
            }
        }

        public async Task<string> GetCachedSecret(string keyName, string cacheName, DateTimeOffset timeToLive, bool update = false) => await GetCachedSecret(KeyVaultUrl, keyName, cacheName, timeToLive, update);
        static object _cacheLock = new object();
        static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { });
        public static IMemoryCache Default
        {
            get
            {
                if (_cache == null)
                    lock (_cacheLock)
                        if (_cache == null)
                            _cache = new MemoryCache(new MemoryCacheOptions() { });
                return _cache;
            }
        }


        public static async Task<string> GetCachedSecret(string baseUrl, string keyName, string cacheName, DateTimeOffset timeToLive, bool update = false, bool forceUpdate = false)
        {
            var secretName = $"{cacheName}-{keyName}";

            string secret = Default.Get<string>(secretName);

            if (secret == null || update)
            {
                secret = await GetSecret(baseUrl, keyName);

                _cache.Set<string>(secretName, secret, timeToLive);
            }
            return secret.ToString();
        }

        public static bool TryGetCachedSecret(string keyName, string cacheName, out string value)
        {
            value = Default.Get<string>(cacheName);
            return value != null;
        }
    }
}
