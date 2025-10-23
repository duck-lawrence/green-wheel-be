using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AppSettingConfigurations
{
    public class RateLimitSettings
    {
        public  int TokenLimit {get; set;} = 20;                // Số token tối đa trong xô
        public  int TokensPerPeriod { get; set; } = 5;            // Số token nạp mỗi chu kỳ
        public  int ReplenishmentPeriod { get; set; } = 10; // Chu kỳ nạp
        public  int QueueLimit { get; set; } = 2;                 // Cho phép tối đa 2 request chờ
    }
}
