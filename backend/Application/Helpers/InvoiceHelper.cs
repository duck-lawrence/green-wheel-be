using Application.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class InvoiceHelper
    {
        public static decimal CalculateTotalAmount(IEnumerable<InvoiceItem> items)
        {
            if (items == null || !items.Any())
                return 0;

            return items.Where(i => i.Type != (int)InvoiceItemType.Penalty && i.Type != (int)InvoiceItemType.Cleaning)
                .Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}
