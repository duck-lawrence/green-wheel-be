using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Invoice.Request
{
    public class UpdateInvoiceReq
    {
        public int PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
