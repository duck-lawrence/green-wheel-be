using Application.AppSettingConfigurations;
using Application.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;


namespace Infrastructure.Repositories
{
    public class OTPRepository : IOTPRepository
    {
        private readonly IDistributedCache _cache;
        private readonly OTPSettings _otpSettings;
       
        public OTPRepository(IDistributedCache cache, IOptions<OTPSettings> otpSettings)
        {
            _cache = cache;
            _otpSettings = otpSettings.Value;
        }
        
        public async Task SaveOTPAsyns(string key, string otp)
        {
            await _cache.SetStringAsync(
                    $"otp:{key}",
                    otp,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_otpSettings.OtpTtl)
                    }
                );


        }
        //Lấy otp từ redis
        public async Task<string?> GetOtpAsync(string key)
        {
            return await _cache.GetStringAsync($"otp:{key}");
        }

        //Xoá otp khi verify xog
        public async Task RemoveOTPAsync(string key)
        {
             await _cache.RemoveAsync($"otp:{key}");
        }

        public async Task<int> CountRateLimitAsync(string email)
        {
            var key = $"otp_rate_limit:{email}";
            var current = await _cache.GetStringAsync(key);
            int count = 0;
            if (!string.IsNullOrEmpty(current))
            {
                count = int.Parse(current);
            }
            count++;
            if (count == 1)
            {
                // lần đầu tiên -> set TTL
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_otpSettings.OtpRateLimitTtl)
                };
                await _cache.SetStringAsync(key, count.ToString(), options);
            }
            else
            {
                // lần sau -> không reset TTL
                await _cache.SetStringAsync(key, count.ToString());
            }

            return count;
        }

        public async Task<int> CountAttemptAsync(string email)
        {
            var key = $"otp_attempt:{email}";
            var current = await _cache.GetStringAsync(key);
            int count = 0;
            if (!string.IsNullOrEmpty(current))
            {
                count = int.Parse(current);
            }
            count++;
            if (count == 1)
            {
                // lần đầu tiên -> set TTL
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_otpSettings.OtpAttemptsTtl)
                };
                await _cache.SetStringAsync(key, count.ToString(), options);
            }
            else
            {
                // lần sau -> không reset TTL
                await _cache.SetStringAsync(key, count.ToString());
            }

            return count;
        }
        public async Task ResetAttemptAsync(string email)
        {
            var key = $"otp_attempt:{email}";
            await _cache.RemoveAsync(key);
        }

       
    }
}
