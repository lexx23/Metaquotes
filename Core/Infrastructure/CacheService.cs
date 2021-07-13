using System;
using System.Collections.Concurrent;
using Common.Services;
using Common.Settings;

namespace Services.Infrastructure
{
    internal class CacheModel<TRes>
    {
        internal TRes Data { get; }
        internal DateTime Expire { get; }

        internal CacheModel(TimeSpan ttl, TRes data)
        {
            Expire = DateTime.UtcNow.Add(ttl);
            Data = data;
        }
    }

    internal class CacheService<TRes, TKey> : ICacheService<TRes, TKey>
    {
        private readonly TimeSpan _ttl;
        private readonly ConcurrentDictionary<TKey, CacheModel<TRes>> _cache;

        public CacheService(CacheOptions cacheOptions)
        {
            _ttl = cacheOptions.CacheTTL;
            _cache = new ConcurrentDictionary<TKey, CacheModel<TRes>>();
        }

        public TRes Cache(TKey key, Func<TRes> execute)
        {
            TRes result;
            if (!_cache.ContainsKey(key))
            {
                result = execute();
                _cache.TryAdd(key, new CacheModel<TRes>(_ttl, result));
                return result;
            }

            // cache exists
            if (_cache.TryGetValue(key, out var cachedData))
            {
                // cashed data not expired
                if (cachedData.Expire > DateTime.UtcNow)
                    return cachedData.Data;
                else
                    _cache.TryRemove(key, out cachedData);
            }

            // expired or cant get cache request new data
            result = execute();
            _cache.TryAdd(key, new CacheModel<TRes>(_ttl, result));
            return result;
        }
    }
}