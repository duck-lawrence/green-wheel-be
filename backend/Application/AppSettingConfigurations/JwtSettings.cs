using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AppSettingConfigurations
{
    public class JwtSettings
    {
        public string AccessTokenKey { get; set; } = "";
        public int AccessTokenExpiredTime { get; set; }
        public string RefreshTokenKey { get; set; } = "";
        public int RefreshTokenExpiredTime { get; set; }
        public string RegisterTokenKey { get; set; } = "";
        public int RegisterTokenExpiredTime { get; set; }
        public string ForgotPasswordTokenKey { get; set; } = "";
        public int ForgotPasswordTokenExpiredTime { get; set; }
        public string SetPasswordTokenKey { get; set; } = "";
        public int SetPasswordTokenExpiredTime { get; set; }
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        
        
    }
}
