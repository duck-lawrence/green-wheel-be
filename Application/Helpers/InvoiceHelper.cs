using Application.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class InvoiceHelper
    {
        public static decimal CalculateTotalAmount(Invoice invoice)
        {
            decimal total = 0;
            if (invoice.Type == (int)InvoiceType.Return)
            {
                var itemLateReturn = invoice.InvoiceItems.Where(it => it.Type == (int)InvoiceItemType.LateReturn).FirstOrDefault();
                total += _CalculateSubTotalAmount([itemLateReturn]);
            }
            if (invoice.Type == (int)InvoiceType.Handover)
            {
                total += invoice.Deposit.Amount;
                
            }
            if(invoice.Type == (int)InvoiceType.Refund)
            { 
                var refund = invoice.InvoiceItems.Where(it => it.Type == (int)InvoiceItemType.Refund).FirstOrDefault();  
                total -= _CalculateSubTotalAmount([refund]);
            }

            total += invoice.Subtotal + invoice.Subtotal * invoice.Tax; 
            return total;
        }

        public static decimal CalculateSubTotalAmount(IEnumerable<InvoiceItem> items)
        {
            if (items == null || !items.Any() || items.Any(x => x == null))
                return 0;
            return items.Where(i =>
            i.Type != (int)InvoiceItemType.LateReturn)
                .Sum(item => item.UnitPrice * item.Quantity);
        }
        //hàm này sẽ dùng để tính nội bộ trong này chứ k dùng ở ngoài
        private static decimal _CalculateSubTotalAmount(IEnumerable<InvoiceItem> items)
        {
            if (items == null || !items.Any() || items.Any(x => x == null))
                return 0;
            return items
                .Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}
