using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AppSettingConfigurations
{
    public class MomoSettings
    {
        public string PartnerCode { get; set; } //momo nhận diện coi mình là danh nghiệp nào
        public string AccessKey { get; set; } //nhận diện ứng dụng gửi request
        public string SecretKey { get; set; } //Được cấp khi đăng kí, dùng để kí
        public string RedirectUrl { get; set; } //Dùng để chuyển về
        public string IpnUrl { get; set; }//nơi nhận status
        public string Endpoint { get; set; }//url của momo để nhận req, đang test nên nó sẽ là sandbox
        public string RequestType { get; set; }
        public string OrderExpireTime { get; set; }
        public string? Lang { get; set; }
        public int Ttl { get; set; }
    }
}
