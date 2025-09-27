using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AppSettingConfigurations
{
    public class OTPSettings
    {
        public int OtpTtl { get; set; }
        public int OtpRateLimit { get; set; }
        public int OtpRateLimitTtl { get; set; }
        public int OtpAttempts { get; set; }
        public int OtpAttemtsTtl { get; set; }

    }
}
