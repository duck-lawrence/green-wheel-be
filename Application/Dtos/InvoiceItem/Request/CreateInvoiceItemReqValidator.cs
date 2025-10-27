using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.InvoiceItem.Request
{
    public class CreateInvoiceItemReqValidator : AbstractValidator<CreateInvoiceItemReq>
    {
        public CreateInvoiceItemReqValidator()
        {
            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("invoice_item.invalid_unit_price");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("invoice_item.invalid_quantity");

            RuleFor(x => x.Type)
                .InclusiveBetween(0, 5).WithMessage("invoice_item.invalid_type");

        }
    }
}
