using Application.Dtos.InvoiceItem.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Invoice.Request
{
    public class CreateInvoiceReq
    {
        public Guid ContractId { get; set; }
        public int Type { get; set; }
        public IEnumerable<CreateInvoiceItemReq> Items { get; set; } = [];
    }
}
