using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Request
{
    public class MomoPaymentReq
    {
        public string PartnerCode { get; set; } = "";
        public string RequestId { get; set; } = "";//Guid tự generate
        public string Amount { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string OrderInfo { get; set; } = "";
        public string RedirectUrl { get; set; } = "";
        public string IpnUrl { get; set; } = "";
        public string RequestType { get; set; } = "payWithMethod";
        public string ExtraData { get; set; } = "";
        public int OrderExpireTime { get; set; } = 30;
        public string Lang { get; set; } = "en";
        public string Signature { get; set; } = "";
    }
}
