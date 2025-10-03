using Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class AutoGenerateHelper
    {
        public static string GenerateInvoiceItemNotes(InvoiceItemType type, int? days = null)
        {
            return type switch
            {
                InvoiceItemType.BaseRental => days.HasValue
                    ? $"Base rental fee for {days.Value} days"
                    : "Base rental fee",

                InvoiceItemType.Damage => "Damage charge based on vehicle checklist",
                InvoiceItemType.LateReturn => "Penalty for late return",
                InvoiceItemType.Cleaning => "Cleaning service fee",
                InvoiceItemType.Penalty => "General penalty fee",
                InvoiceItemType.Other => "Additional charge",

                _ => "Invoice item" //case default
            };
        }
    }
}
