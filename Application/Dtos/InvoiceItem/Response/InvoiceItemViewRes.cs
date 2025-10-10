using Application.Constants;
using Application.Dtos.VehicleChecklistItem.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.InvoiceItem.Response
{
    public class InvoiceItemViewRes
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public int Type { get; set; } = (int)InvoiceItemType.BaseRental;
        public VehicleChecklistItemViewRes? ChecklistItem { get; set; } = null;
    }
}
