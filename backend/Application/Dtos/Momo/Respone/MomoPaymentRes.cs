using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Respone
{
    public class MomoPaymentRes
    {
        public string PartnerCode { get; set; } = "";
        public string OrderId { get; set; } = "";
        public string RequestId { get; set; } = "";
        public long Amount { get; set; }
        public string Message { get; set; } = "";
        public int ResultCode { get; set; } 
        public string PayUrl { get; set; } = "";
        public string ShortLink { get; set; } = "";
    }
}
