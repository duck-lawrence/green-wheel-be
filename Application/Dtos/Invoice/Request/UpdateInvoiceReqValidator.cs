using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Dtos.Invoice.Request
{
    public class UpdateInvoiceReqValidator : AbstractValidator<UpdateInvoiceReq>
    {
        public UpdateInvoiceReqValidator()
        {
            RuleFor(x => x.PaymentMethod)
                .InclusiveBetween(0, 1).WithMessage("invoice.invalid_payment_method");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage(InvoiceMessage.AmountRequired);
        }
    }
}
