using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class VerifyOTPReq
    {
        public string OTP { get; set; }
        public string Email { get; set; }

    }
}
