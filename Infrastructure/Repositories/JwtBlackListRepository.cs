using Application.AppSettingConfigurations;
using Application.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Repositories
{
    public class JwtBlackListRepository : IJwtBlackListRepository
    {
        private readonly IDistributedCache _cache;
        public JwtBlackListRepository(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<bool> CheckTokenInBlackList(string key)
        {
            return (await _cache.GetStringAsync($"jwtblacklist:{key}")) != null;
        }

        public async Task SaveTokenAsyns(string key, long ttl)
        {
            DateTimeOffset tokenExpiredTime = DateTimeOffset.FromUnixTimeSeconds(ttl);
            TimeSpan timeToLive =  tokenExpiredTime- DateTimeOffset.UtcNow ;
            int seconds = (int)timeToLive.TotalSeconds;
            if(seconds > 0)
            {
                await _cache.SetStringAsync(
                    $"jwtblacklist:{key}",
                    "1",
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = timeToLive
                    }
                );
            }
            
        }
    }
}
