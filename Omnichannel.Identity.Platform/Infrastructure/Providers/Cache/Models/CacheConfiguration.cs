namespace Omnichannel.Identity.Platform.Infrastructure.Providers.Cache.Models
{
    public sealed class CacheConfiguration
    {
        public string CacheKey { get; private set; }

        public CacheConfiguration(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        public string GetCacheKey(string company, string key)
        {
            return string.Format(CacheKey, company, key);
        }
    }
}
