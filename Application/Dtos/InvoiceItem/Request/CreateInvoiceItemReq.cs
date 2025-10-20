using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.InvoiceItem.Request
{
    public class CreateInvoiceItemReq
    {
        public decimal UnitPrice { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public int Quantity { get; set; }
    }
}
