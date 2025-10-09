using Application.Constants;
using Application.Dtos.Deposit.Respone;
using Application.Dtos.InvoiceItem.Response;
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
        public IEnumerable<InvoiceItemViewRes> InvoiceItems { get; set; }
        public DepositViewRes? Deposit { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; }
        public int? PaymentMethod { get; set; } = null;
        public string? Notes { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
        public DateTimeOffset? PaidAt { get; set; } = null;
        public Guid? CheckListId { get; set; } = null;

    }
}
