using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IOTPRepository
    {
        Task RemoveOTPAsync(string key);
        Task<string?> GetOtpAsync(string key);
        Task SaveOTPAsyns(string key, string otp);
        Task<int> CountAttemptAsync(string email);
        Task<int> CountRateLimitAsync(string email);
        Task ResetAttemptAsync(string email);


    }
}
