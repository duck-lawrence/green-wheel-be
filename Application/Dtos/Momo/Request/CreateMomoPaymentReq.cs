using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Momo.Request
{
    public class CreateMomoPaymentReq
    {
        public decimal Amount { get; set; }
        public Guid InvoiceId { get; set; }
        public string Description { get; set; }
    }
}
