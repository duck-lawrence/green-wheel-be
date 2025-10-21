using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Request
{
    public class CreateMomoPaymentReq
    {
        public Guid InvoiceId { get; set; }
        public string FallbackUrl { get; set; } = null!;
    }
}
