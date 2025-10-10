using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class GenerateOtpHelper
    {
        private static readonly Random random = new Random();
        public static string GenerateOtp()
        {
            var otp = random.Next(0, 1000000); //từ 0 đến 999999
            return otp.ToString("D6");
        }
    }
}
