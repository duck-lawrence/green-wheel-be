using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Invoice.Response
{
    public class InvoiceViewRes
    {
        public Guid Id { get; set; }
        public IEnumerable<InvoiceItemViewRes>  { get; set; }
    }
}
